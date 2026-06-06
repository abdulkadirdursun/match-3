using Match3.ObjectPooling;
using UnityEngine;

namespace Match3.Core
{
    public class GameBoardCreator : MonoBehaviour
    {
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private BoardConfig boardConfig;
        [Header("Pool")]
        [SerializeField] private BoardCell boardCellPrefab;
        [SerializeField] private int startPoolSize = 16;
        [SerializeField] private int maxPoolSize = 144;

        private ObjectPool<BoardCell> _cellPool;

        public void GenerateBoard()
        {
            var boardSize = gameplayData.LevelData.BoardSize;
            var cellSize = boardConfig.CellSize;
            var halfExtendX = (boardSize.x - 1) * cellSize * 0.5f;
            var halfExtendY = (boardSize.y - 1) * cellSize * 0.5f;
            var boardCells = new BoardCell[boardSize.x, boardSize.y];
            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    var coords = new Vector2Int(x, y);
                    var position = new Vector3(
                        (x * cellSize) - halfExtendX,
                        (y * cellSize) - halfExtendY,
                        0f);
                    var cell = _cellPool.Get();
#if UNITY_EDITOR || DEBUG_BUILD
                    cell.name = $"BoardCell ({x}, {y})";
#endif
                    cell.Initialize(position, coords, cellSize);
                    boardCells[x, y] = cell;
                }
            }

            gameBoardData.SetLevelBoard(boardCells);
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            _cellPool = new ObjectPool<BoardCell>(boardCellPrefab, transform, startPoolSize, maxPoolSize);
            GenerateBoard();//TODO: Temp call
        }

        #endregion
    }
}