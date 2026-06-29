using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.Core.Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Core
{
    public class BoardResolver : MonoBehaviour
    {
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private MatchScanner matchScanner;
        [SerializeField] private BoardItemSpawner boardItemSpawner;

        private const int MaxMatchCheckIteration = 64;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _resolving;
        private bool _boardDirty;
        private bool _swapping;

        public void SwapCellItems(BoardCell originCell, BoardCell movedCell)
        {
            if (_swapping) return;
            SwapCellItemsAsync(originCell, movedCell).Forget();
        }

        public void AbortTasks()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            RefreshCancellationTokenSource();
            _resolving = false;
            _boardDirty = false;
            _swapping = false;
        }

        private async UniTaskVoid SwapCellItemsAsync(BoardCell originCell, BoardCell movedCell)
        {
            _swapping = true;
            try
            {
                var token = _cancellationTokenSource.Token;
                var originCellItem = originCell.BoardItem;
                var movedCellItem = movedCell.BoardItem;
                originCell.SetItem(movedCellItem);
                movedCell.SetItem(originCellItem);

                if (!matchScanner.HasMatchAt(originCell.Coordinates)
                    && !matchScanner.HasMatchAt(movedCell.Coordinates))
                {
                    UniTask[] tasks = new[]
                    {
                        originCellItem.BounceToAndBack(movedCell.WorldPos, originCell.WorldPos).ToUniTask(cancellationToken: token),
                        movedCellItem.BounceToAndBack(originCell.WorldPos, movedCell.WorldPos).ToUniTask(cancellationToken: token)
                    };
                    await UniTask.WhenAll(tasks);
                    originCell.SetItem(originCellItem);
                    movedCell.SetItem(movedCellItem);
                }
                else
                {
                    UniTask[] tasks = new[]
                    {
                        originCellItem.MoveToPos(movedCell.WorldPos).ToUniTask(cancellationToken: token),
                        movedCellItem.MoveToPos(originCell.WorldPos).ToUniTask(cancellationToken: token)
                    };
                    await UniTask.WhenAll(tasks);
                    ResolveMatchesAsync().Forget();
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _swapping = false;
            }
        }

        private async UniTaskVoid ResolveMatchesAsync()
        {
            if (_resolving)
            {
                _boardDirty = true;
                return;
            }

            _resolving = true;
            try
            {
                var token = _cancellationTokenSource.Token;
                var tasks = new List<UniTask>();
                for (int i = 0; i < MaxMatchCheckIteration; i++)
                {
                    var matches = matchScanner.FindAllMatches();
                    if (matches.Count == 0)
                    {
                        if (_boardDirty)
                        {
                            _boardDirty = false;
                            continue;
                        }

                        break;
                    }

                    tasks.Clear();
                    foreach (var cell in matches)
                    {
                        var item = cell.DetachItem();
                        tasks.Add(item.BreakItem().ToUniTask(cancellationToken: token));
                    }

                    if (tasks.Count > 0) await UniTask.WhenAll(tasks);

                    tasks.Clear();
                    foreach (var collapseTween in CollapseBoard())
                        tasks.Add(collapseTween.ToUniTask(cancellationToken: token));
                    foreach (var refillTween in boardItemSpawner.RefillEmptyCells())
                        tasks.Add(refillTween.ToUniTask(cancellationToken: token));

                    if (tasks.Count > 0) await UniTask.WhenAll(tasks);

                    if (!matchScanner.HasValidMove())
                    {
                        tasks.Clear();
                        foreach (var shuffleTween in ShuffleBoard())
                            tasks.Add(shuffleTween.ToUniTask(cancellationToken: token));
                        if (tasks.Count > 0) await UniTask.WhenAll(tasks);
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _resolving = false;
            }
        }

        private List<Tween> CollapseBoard()
        {
            var tweens = new List<Tween>();
            var boardSize = gameBoardData.BoardSize;

            for (int x = 0; x < boardSize.x; x++)
            {
                int writeY = 0;

                for (int readY = 0; readY < boardSize.y; readY++)
                {
                    if (readY <= writeY
                        || !gameBoardData.TryGetBoardCell(new Vector2Int(x, writeY), out var writeCell)) continue;

                    if (!writeCell.HasBoardItem)
                    {
                        if (!gameBoardData.TryGetBoardCell(new Vector2Int(x, readY), out var readCell)
                            || !readCell.HasBoardItem) continue;

                        var item = readCell.DetachItem();
                        writeCell.SetItem(item);
                        var tween = item.FallToPos(writeCell.WorldPos);
                        tweens.Add(tween);
                    }

                    writeY++;
                }
            }

            return tweens;
        }

        private List<Tween> ShuffleBoard()
        {
            const int maxIteration = 50;
            var occupiedCells = new List<BoardCell>();
            var items = new List<BoardItem>();
            var tweens = new List<Tween>();
            gameBoardData.GetOccupiedCells(occupiedCells);

            if (!HasEnoughSameTypeItemToMatch(occupiedCells, out var itemTypeCounts))
            {
                boardItemSpawner.SpawnBoardItems();
                return tweens;
            }

            for (int i = 0; i < maxIteration; i++)
            {
                items.Clear();
                foreach (var cell in occupiedCells)
                    items.Add(cell.DetachItem());

                for (int j = items.Count - 1; j >= 0; j--)
                {
                    var randomIndex = Random.Range(0, j + 1);
                    (items[j], items[randomIndex]) = (items[randomIndex], items[j]);
                }

                for (int cellIndex = 0; cellIndex < occupiedCells.Count; cellIndex++)
                    occupiedCells[cellIndex].SetItem(items[cellIndex]);

                if (matchScanner.HasValidMove())
                {
                    foreach (var cell in occupiedCells)
                        tweens.Add(cell.BoardItem.MoveToPos(cell.WorldPos));

                    return tweens;
                }
            }

            //RebuildWithGuaranteedMove
            BoardItemData seedItemData = null;
            foreach (var itemTypeCount in itemTypeCounts)
            {
                if (itemTypeCount.Value < 3) continue;
                seedItemData = itemTypeCount.Key;
                break;
            }

            var seedCells = FindSeedTriple(occupiedCells);
            var seedItems = new List<BoardItem>();
            items.Clear();
            for (int i = occupiedCells.Count - 1; i >= 0; i--)
            {
                var boardCell = occupiedCells[i];
                if (boardCell.BoardItem.BoardItemData != seedItemData || seedItems.Count == 3)
                {
                    items.Add(boardCell.DetachItem());
                    continue;
                }

                seedItems.Add(boardCell.DetachItem());
            }

            int seedIndex = 0;
            foreach (var cell in seedCells)
            {
                var item = seedItems[seedIndex];
                cell.SetItem(item);
                tweens.Add(item.MoveToPos(cell.WorldPos));
                seedIndex++;
            }

            foreach (var cell in occupiedCells)
            {
                if (seedCells.Contains(cell))
                    continue;

                var item = items.SelectRandomAndRemove();
                cell.SetItem(item);
                tweens.Add(item.MoveToPos(cell.WorldPos));
            }

            return tweens;
        }

        private bool HasEnoughSameTypeItemToMatch(List<BoardCell> occupiedCells, out Dictionary<BoardItemData, int> itemTypeCounts)
        {
            itemTypeCounts = new Dictionary<BoardItemData, int>();
            var hasEnoughSameTypeItem = false;
            foreach (var cell in occupiedCells)
            {
                var itemData = cell.BoardItem.BoardItemData;
                if (itemTypeCounts.TryGetValue(itemData, out var count))
                {
                    var newCount = count + 1;
                    itemTypeCounts[itemData] = newCount;
                    if (!hasEnoughSameTypeItem)
                        hasEnoughSameTypeItem = newCount >= 3;
                }
                else
                {
                    itemTypeCounts.Add(itemData, 1);
                }
            }

            return hasEnoughSameTypeItem;
        }

        private HashSet<BoardCell> FindSeedTriple(List<BoardCell> cells)
        {
            var selectedCells = new HashSet<BoardCell>();
            for (int i = 0; i < cells.Count; i++)
            {
                selectedCells.Clear();
                var cell1 = cells[i];
                selectedCells.Add(cell1);
                if (gameBoardData.TryGetBoardCell(cell1.Coordinates + Vector2Int.right, out var horizontalCell2)
                    && horizontalCell2.HasBoardItem
                    && gameBoardData.TryGetBoardCell(horizontalCell2.Coordinates + (2 * Vector2Int.right), out var horizontalCell3)
                    && horizontalCell3.HasBoardItem)
                {
                    selectedCells.Add(horizontalCell2);
                    selectedCells.Add(horizontalCell3);
                    break;
                }

                //Vertical Check
                if (gameBoardData.TryGetBoardCell(cell1.Coordinates + Vector2Int.up, out var verticalCell2)
                    && verticalCell2.HasBoardItem
                    && gameBoardData.TryGetBoardCell(verticalCell2.Coordinates + (2 * Vector2Int.up), out var verticalCell3)
                    && verticalCell3.HasBoardItem)
                {
                    selectedCells.Add(verticalCell2);
                    selectedCells.Add(verticalCell3);
                    break;
                }
            }

            return selectedCells;
        }

        private void RefreshCancellationTokenSource()
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            RefreshCancellationTokenSource();
        }

        #endregion
    }
}