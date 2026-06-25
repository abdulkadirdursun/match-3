using System.Collections.Generic;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "GameBoardData", menuName = "Match 3/Core/Game Board Data")]
    public class GameBoardData : ScriptableObject
    {
        [SerializeField] private BoardConfig boardConfig;

        public Vector2Int BoardSize { get; private set; }
        private BoardCell[,] _boardCells;

        public void SetBoardSize(Vector2Int boardSize)
        {
            BoardSize = boardSize;
        }

        public void SetLevelBoard(BoardCell[,] boardCells)
        {
            _boardCells = boardCells;
        }

        public void GetOccupiedCells(List<BoardCell> occupiedCells)
        {
            occupiedCells.Clear();
            foreach (var boardCell in _boardCells)
            {
                if (!boardCell.HasBoardItem) continue;
                occupiedCells.Add(boardCell);
            }
        }

        public bool TryGetBoardCell(Vector2Int coords, out BoardCell boardCell)
        {
            boardCell = null;
            if (_boardCells == null
                || coords.x < 0 || coords.x >= BoardSize.x
                || coords.y < 0 || coords.y >= BoardSize.y) return false;
            boardCell = _boardCells[coords.x, coords.y];
            return true;
        }

        public bool TryGetBoardCell(Vector3 worldPos, out BoardCell boardCell)
        {
            boardCell = null;
            if (_boardCells == null) return false;
            var halfExtendX = BoardSize.x * boardConfig.CellSize * 0.5f;
            var halfExtendY = BoardSize.y * boardConfig.CellSize * 0.5f;

            var coord = Vector2Int.zero;
            coord.x = Mathf.FloorToInt((worldPos.x + halfExtendX) / boardConfig.CellSize);
            coord.y = Mathf.FloorToInt((worldPos.y + halfExtendY) / boardConfig.CellSize);
            return TryGetBoardCell(coord, out boardCell);
        }
    }
}