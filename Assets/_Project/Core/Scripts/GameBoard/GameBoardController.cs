using Match3.Core.BoardItems;
using UnityEngine;

namespace Match3.Core.GameBoard
{
    public class GameBoardController : MonoBehaviour
    {
        [SerializeField] private BoardItemSpawner boardItemSpawner;
        [SerializeField] private Vector2Int boardSize = new Vector2Int(8, 8);
        [SerializeField] private float cellSize = 1f;
        [SerializeField] private BoardCell boardCell;

        private BoardCell[] _cells;

        private void CreateTheBoard()
        {
            ClearTheBoard();
            _cells = new BoardCell[boardSize.x * boardSize.y];
            var halfExtendX = (boardSize.x - 1) * cellSize * 0.5f;
            var halfExtendY = (boardSize.y - 1) * cellSize * 0.5f;

            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    var arrayIndex = y * boardSize.x + x;
                    var localPosX = (x * cellSize) - halfExtendX;
                    var localPosY = (y * cellSize) - halfExtendY;
                    var cell = Instantiate(boardCell, transform);
                    cell.Initialize(new Vector3(localPosX, localPosY, 0f), new Vector2Int(x, y), cellSize);
                    _cells[arrayIndex] = cell;
                }
            }
            
            boardItemSpawner.SpawnBoardItems(_cells);//temp method call
        }

        private void ClearTheBoard()
        {
            if (_cells == null) return;
            for (int i = _cells.Length - 1; i >= 0; i--)
            {
                Destroy(_cells[i].gameObject);
            }

            _cells = null;
        }

        #region MonoBehaviour Methods

        private void Start()
        {
            CreateTheBoard();
        }

        #endregion
    }
}