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

        public async void SwapCellItems(BoardCell originCell, BoardCell movedCell)
        {
            var originCellItem = originCell.BoardItem;
            var movedCellItem = movedCell.BoardItem;
            var originCellItemMoveTween = originCellItem.MoveToPos(movedCell.WorldPos, boardAnimationConfig.SwapDuration, boardAnimationConfig.SwapEase);
            var movedCellItemMoveTween = movedCellItem.MoveToPos(originCell.WorldPos, boardAnimationConfig.SwapDuration, boardAnimationConfig.SwapEase);
            originCell.SetItem(movedCellItem);
            movedCell.SetItem(originCellItem);
            await Task.WhenAll(originCellItemMoveTween.AsyncWaitForCompletion(), movedCellItemMoveTween.AsyncWaitForCompletion());
            if (!matchScanner.HasMatchAt(originCell.Coordinates)
                && !matchScanner.HasMatchAt(movedCell.Coordinates))
            {
                originCellItemMoveTween = originCellItem.MoveToPos(originCell.WorldPos, boardAnimationConfig.SwapDuration, boardAnimationConfig.SwapEase);
                movedCellItemMoveTween = movedCellItem.MoveToPos(movedCell.WorldPos, boardAnimationConfig.SwapDuration, boardAnimationConfig.SwapEase);
                originCell.SetItem(originCellItem);
                movedCell.SetItem(movedCellItem);
                await Task.WhenAll(originCellItemMoveTween.AsyncWaitForCompletion(), movedCellItemMoveTween.AsyncWaitForCompletion());
            }
            else
            {
                ResolveMatches();
            }
        }

        public async void ResolveMatches()
        {
            if (_resolving) return;
            _resolving = true;
            var tweens = new List<Tween>();
            for (int i = 0; i < MaxMatchCheckIteration; i++)
            {
                var matches = matchScanner.FindAllMatches();
                if (matches.Count == 0) break;
                foreach (var cell in matches)
                    cell.ClearInPlace();

                tweens.AddRange(CollapseBoard());
                tweens.AddRange(boardItemSpawner.RefillEmptyCells());
                if (tweens.Count > 0)
                    await Task.WhenAll(tweens.Select(t => t.AsyncWaitForCompletion()));
            }

            _resolving = false;
        }

        private List<Tween> CollapseBoard()
        {
            var tweens = new List<Tween>();
            var boardSize = gameplayData.LevelData.BoardSize;

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
                        var tween = item.MoveToPos(writeCell.WorldPos, boardAnimationConfig.FallDuration, boardAnimationConfig.FallEase);
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