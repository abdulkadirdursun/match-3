using UnityEngine;

namespace Match3.Core
{
    public class GameBoardController : MonoBehaviour
    {
        [SerializeField] private BoardItemSpawner boardItemSpawner;
        [SerializeField] private MatchController matchController;
        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private BoardCell boardCellPrefab;
        [SerializeField] private Vector2Int boardSize = new Vector2Int(8, 8); //temp

        public BoardCell[,] BoardCells { get; private set; }
        public Vector2Int BoardSize => boardSize;

        public bool TryToGetCellAtCoord(Vector2 worldPos, out BoardCell boardCell)
        {
            boardCell = null;
            var cellSize = boardConfig.CellSize;
            var halfExtendX = boardSize.x * cellSize * 0.5f;
            var halfExtendY = boardSize.y * cellSize * 0.5f;

            var coordX = Mathf.FloorToInt((worldPos.x + halfExtendX) / cellSize);
            var coordY = Mathf.FloorToInt((worldPos.y + halfExtendY) / cellSize);

            if (coordX < 0 || coordX >= boardSize.x || coordY < 0 || coordY >= boardSize.y)
                return false;

            boardCell = BoardCells[coordX, coordY];
            return true;
        }

        public void SwapItems(BoardCell firstCell, BoardCell secondCell)
        {
            var firstBoardItem = firstCell.BoardItem;
            var secondBoardItem = secondCell.BoardItem;
            firstCell.SetItem(secondBoardItem);
            secondCell.SetItem(firstBoardItem);
            var firstCellMatched = matchController.CheckForMatch(firstCell.Coordinates);
            var secondCellMatched = matchController.CheckForMatch(secondCell.Coordinates);
            if (firstCellMatched || secondCellMatched) return;
            firstCell.SetItem(firstBoardItem);
            secondCell.SetItem(secondBoardItem);
        }

        private void CreateTheBoard()
        {
            BoardCells = new BoardCell[boardSize.x, boardSize.y];
            var halfExtendX = (boardSize.x - 1) * boardConfig.CellSize * 0.5f;
            var halfExtendY = (boardSize.y - 1) * boardConfig.CellSize * 0.5f;

            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    var localPosX = (x * boardConfig.CellSize) - halfExtendX;
                    var localPosY = (y * boardConfig.CellSize) - halfExtendY;
                    var cell = Instantiate(boardCellPrefab, transform);
#if UNITY_EDITOR || DEBUG_BUILD
                    cell.name = $"BoardCell ({x}, {y})";
#endif
                    cell.Initialize(new Vector3(localPosX, localPosY, 0f), new Vector2Int(x, y), boardConfig.CellSize);
                    BoardCells[x, y] = cell;
                }
            }

            boardItemSpawner.SpawnBoardItems(BoardCells);
        }


        #region MonoBehaviour Methods

        private void Start()
        {
            CreateTheBoard();
        }

        #endregion
    }
}