using Match3.Core.Helpers;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItemSpawner : MonoBehaviour
    {
        [SerializeField] private BoardItem boardItemPrefab;
        [SerializeField] private BoardItemData[] boardItemDataArray;

        public void SpawnBoardItems(BoardCell[,] boardCells) //temp method
        {
            foreach (var cell in boardCells)
            {
                var newBoardItem = Instantiate(boardItemPrefab, transform);
                newBoardItem.Initialize(boardItemDataArray.Random());
                cell.SetItem(newBoardItem);
            }
        }
    }
}