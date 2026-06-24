using Match3.ObjectPooling;
using UnityEngine;

namespace Match3.Core
{
    public class GameBoardCreator : MonoBehaviour
    {
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private Transform maskTransform;
        [Header("Pool")]
        [SerializeField] private BoardCell boardCellPrefab;
        [SerializeField] private int startPoolSize = 16;
        [SerializeField] private int maxPoolSize = 144;

        private ObjectPool<BoardCell> _cellPool;

        public void GenerateBoard()
        {
            _cellPool ??= new ObjectPool<BoardCell>(boardCellPrefab, transform, startPoolSize, maxPoolSize, onRelease: OnRelease);
            _cellPool.ReleaseAll();
            var boardSize = gameplayData.BoardSize;
            var cellSize = boardConfig.CellSize;
            maskTransform.localScale = new Vector3(boardSize.x * cellSize, boardSize.y * cellSize, 1f);
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

        #region Pool Methods

        private void OnRelease(BoardCell boardCell)
        {
            boardCell.Reset();
        }

        #endregion
    }
}