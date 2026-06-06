using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardResolver : MonoBehaviour
    {
        [SerializeField] private MatchScanner matchScanner;

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
            var matches = matchScanner.FindAllMatches();
            if (matches.Count == 0) return;
            foreach (var cell in matches)
                cell.ClearInPlace();
        }
    }
}