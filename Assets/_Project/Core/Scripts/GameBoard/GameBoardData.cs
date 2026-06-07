using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "GameBoardData", menuName = "Match 3/Core/Game Board Data")]
    public class GameBoardData : ScriptableObject
    {
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private BoardConfig boardConfig;

        private BoardCell[,] _boardCells;

        public void SetLevelBoard(BoardCell[,] boardCells)
        {
            _boardCells = boardCells;
        }
        
        public bool TryGetBoardCell(Vector2Int coords, out BoardCell boardCell)
        {
            boardCell = null;
            var boardSize = gameplayData.BoardSize;
            if (_boardCells == null
                || coords.x < 0 || coords.x >= boardSize.x
                || coords.y < 0 || coords.y >= boardSize.y) return false;
            boardCell = _boardCells[coords.x, coords.y];
            return true;
        }

        public bool TryGetBoardCell(Vector3 worldPos, out BoardCell boardCell)
        {
            boardCell = null;
            if (_boardCells == null) return false;
            var boardSize = gameplayData.BoardSize;
            var halfExtendX = boardSize.x * boardConfig.CellSize * 0.5f;
            var halfExtendY = boardSize.y * boardConfig.CellSize * 0.5f;

            var coord = Vector2Int.zero;
            coord.x = Mathf.FloorToInt((worldPos.x + halfExtendX) / boardConfig.CellSize);
            coord.y = Mathf.FloorToInt((worldPos.y + halfExtendY) / boardConfig.CellSize);
            return TryGetBoardCell(coord, out boardCell);
        }
    }
}