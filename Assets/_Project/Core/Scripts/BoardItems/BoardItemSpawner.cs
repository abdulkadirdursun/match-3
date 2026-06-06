using Match3.Core.Helpers;
using Match3.ObjectPooling;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItemSpawner : MonoBehaviour
    {
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private BoardItemData[] boardItemDataArray;
        [Header("Pool")]
        [SerializeField] private BoardItem boardItemPrefab;
        [SerializeField] private int startItemPoolSize = 16;
        [SerializeField] private int maxItemPoolSize = 144;

        private ObjectPool<BoardItem> _boardItemPool;

        public void SpawnBoardItems()
        {
            var boardSize = gameplayData.LevelData.BoardSize;
            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    var coord = new Vector2Int(x, y);
                    if (!gameBoardData.TryGetBoardCell(coord, out var cell)) continue;
                    var newBoardItem = _boardItemPool.Get();
                    newBoardItem.Initialize(boardItemDataArray.Random());
                    newBoardItem.PlaceAt(cell.WorldPos);
                    cell.SetItem(newBoardItem);
                }
            }
        }

        #region Pool Methods

        private void OnCreate(BoardItem boardItem, ObjectPool<BoardItem> pool)
        {
            boardItem.ObjectHid += pool.ReleaseRequest;
        }

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            _boardItemPool = new ObjectPool<BoardItem>(boardItemPrefab, transform, startItemPoolSize, maxItemPoolSize, OnCreate);
        }

        private void Start()
        {
            SpawnBoardItems(); //TODO: Temp function call
        }

        #endregion
    }
}