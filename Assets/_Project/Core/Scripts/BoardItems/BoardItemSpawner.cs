using Match3.Core.Utilities;
using UnityEngine;

namespace Match3.Core.BoardItems
{
    public class BoardItemSpawner : MonoBehaviour
    {
        [SerializeField] private CurrentGameBoardData currentGameBoardData;
        [SerializeField] private BoardItem boardItemPrefab;
        [SerializeField] private BoardItemData[] boardItemDataArray;

        private void SpawnBoardItems() //temp method
        {
            var cells = currentGameBoardData.BoardCells;
            var boardItems = new BoardItem[cells.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                var newBoardItem = Instantiate(boardItemPrefab, transform);
                newBoardItem.Initialize(boardItemDataArray.Random());
                var cell = cells[i];
                cell.SetItem(newBoardItem);
                boardItems[i] = newBoardItem;
            }

            currentGameBoardData.SetBoardItems(boardItems);
        }

        #region OnEnable

        private void OnEnable()
        {
            currentGameBoardData.BoardSetComplete += SpawnBoardItems;
        }

        private void OnDisable()
        {
            currentGameBoardData.BoardSetComplete -= SpawnBoardItems;
        }

        #endregion
    }
}