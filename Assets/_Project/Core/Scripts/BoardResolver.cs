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
        [SerializeField] private MatchScanner matchScanner;
        [SerializeField] private BoardItemSpawner boardItemSpawner;

        private const int MaxMatchCheckIteration = 64;

        public async void SwapCellItems(BoardCell originCell, BoardCell movedCell)
        {
            var originCellItem = originCell.BoardItem;
            var movedCellItem = movedCell.BoardItem;
            var originCellItemMoveTween = originCellItem.MoveToPos(movedCell.WorldPos, 0.2f); // TODO: Temp value for time
            var movedCellItemMoveTween = movedCellItem.MoveToPos(originCell.WorldPos, 0.2f); // TODO: Temp value for time
            originCell.SetItem(movedCellItem);
            movedCell.SetItem(originCellItem);
            await Task.WhenAll(originCellItemMoveTween.AsyncWaitForCompletion(), movedCellItemMoveTween.AsyncWaitForCompletion());
            if (!matchScanner.HasMatchAt(originCell.Coordinates)
                && !matchScanner.HasMatchAt(movedCell.Coordinates))
            {
                originCellItemMoveTween = originCellItem.MoveToPos(originCell.WorldPos, 0.2f); // TODO: Temp value for time
                movedCellItemMoveTween = movedCellItem.MoveToPos(movedCell.WorldPos, 0.2f); // TODO: Temp value for time
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
                        var tween = item.MoveToPos(writeCell.WorldPos, 0.2f).SetEase(Ease.InOutSine); // TODO: Temp value for time
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