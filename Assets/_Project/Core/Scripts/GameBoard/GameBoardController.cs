using System.Threading.Tasks;
using DG.Tweening;
using Match3.InputSystem;
using UnityEngine;

namespace Match3.Core
{
    public class GameBoardController : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private MatchScanner matchScanner;
        
        public async void SwapItems(BoardCell firstCell, BoardCell secondCell)
        {
            var firstBoardItem = firstCell.BoardItem;
            var secondBoardItem = secondCell.BoardItem;
            var firstCellMoveTween = firstCell.SetItem(secondBoardItem);
            var secondCellMoveTween = secondCell.SetItem(firstBoardItem);
            playerInputHandler.SetActivity(false);
            await Task.WhenAll(firstCellMoveTween.AsyncWaitForCompletion(), secondCellMoveTween.AsyncWaitForCompletion());

            var firstCellMatched = matchScanner.HasMatchAt(firstCell.Coordinates);
            var secondCellMatched = matchScanner.HasMatchAt(secondCell.Coordinates);
            if (!firstCellMatched && !secondCellMatched)
            {
                firstCellMoveTween = firstCell.SetItem(firstBoardItem);
                secondCellMoveTween = secondCell.SetItem(secondBoardItem);
                await Task.WhenAll(firstCellMoveTween.AsyncWaitForCompletion(), secondCellMoveTween.AsyncWaitForCompletion());
            }

            playerInputHandler.SetActivity(true);
        }
    }
}