using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardResolver : MonoBehaviour
    {
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private BoardAnimationConfig boardAnimationConfig;
        [SerializeField] private MatchScanner matchScanner;
        [SerializeField] private BoardItemSpawner boardItemSpawner;

        private const int MaxMatchCheckIteration = 64;
        private bool _resolving;
        private bool _boardDirty;

        public void SwapCellItems(BoardCell originCell, BoardCell movedCell)
        {
            var originCellItem = originCell.BoardItem;
            var movedCellItem = movedCell.BoardItem;
            originCell.SetItem(movedCellItem);
            movedCell.SetItem(originCellItem);

            if (!matchScanner.HasMatchAt(originCell.Coordinates)
                && !matchScanner.HasMatchAt(movedCell.Coordinates))
            {
                originCellItem.BounceToAndBack(
                    movedCell.WorldPos,
                    originCell.WorldPos);
                movedCellItem.BounceToAndBack(
                    originCell.WorldPos,
                    movedCell.WorldPos);
                originCell.SetItem(originCellItem);
                movedCell.SetItem(movedCellItem);
            }
            else
            {
                originCellItem.MoveToPos(movedCell.WorldPos);
                movedCellItem.MoveToPos(originCell.WorldPos);
                ResolveMatches();
            }
        }

        private async void ResolveMatches()
        {
            if (_resolving)
            {
                _boardDirty = true;
                return;
            }

            _resolving = true;
            var tweens = new List<Tween>();
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

                foreach (var cell in matches)
                {
                    var item = cell.DetachItem();
                    tweens.Add(item.HideRequest());
                }

                if (tweens.Count > 0) await Task.WhenAll(tweens.Select(t => t.AsyncWaitForCompletion()));


                tweens.Clear();
                tweens.AddRange(CollapseBoard());
                tweens.AddRange(boardItemSpawner.RefillEmptyCells());
                if (tweens.Count > 0) await Task.WhenAll(tweens.Select(t => t.AsyncWaitForCompletion()));
            }

            _resolving = false;
        }

        private List<Tween> CollapseBoard()
        {
            var tweens = new List<Tween>();
            var boardSize = gameplayData.BoardSize;

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
                        var tween = item.FallToPos(writeCell.WorldPos);
                        writeCell.SetItem(item);
                        tweens.Add(tween);
                    }

                    writeY++;
                }
            }

            return tweens;
        }
    }
}