using Match3.Core.GameBoard;
using Match3.Core.Utilities;
using UnityEngine;

namespace Match3.Core.BoardItems
{
    public class BoardItemSpawner : MonoBehaviour
    {
        [SerializeField] private BoardItem boardItemPrefab;
        [SerializeField] private BoardItemData[] boardItems;

        private BoardItem[] _spawnedItems;

        public void SpawnBoardItems(BoardCell[] cells) //temp method
        {
            ClearBoardItems();
            _spawnedItems = new BoardItem[cells.Length];

            for (int i = 0; i < cells.Length; i++)
            {
                var newBoardItem = Instantiate(boardItemPrefab, transform);
                newBoardItem.Initialize(boardItems.Random());
                var cell = cells[i];
                cell.SetItem(newBoardItem);
                _spawnedItems[i] = newBoardItem;
            }
        }

        private void ClearBoardItems()
        {
            if (_spawnedItems == null) return;

            for (int i = _spawnedItems.Length - 1; i >= 0; i--)
            {
                Destroy(_spawnedItems[i].gameObject);
            }

            _spawnedItems = null;
        }
    }
}