using System;
using Match3.Core.BoardItems;
using Match3.Core.GameBoard;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "CurrentGameBoardData", menuName = "Match 3/Core/Current Game Board Data")]
    public class CurrentGameBoardData : ScriptableObject
    {
        public Vector2Int CurrentBoardSize { get; private set; }
        public BoardCell[] BoardCells { get; private set; }

        public BoardItem[] BoardItems { get; private set; }

        public event Action BoardSetComplete;
        public event Action BoardItemSetComplete;

        public void SetBoard(Vector2Int boardSize, BoardCell[] cells)
        {
            CurrentBoardSize = boardSize;
            BoardCells = cells;
            BoardSetComplete?.Invoke();
        }

        public void SetBoardItems(BoardItem[] boardItems)
        {
            BoardItems = boardItems;
            BoardItemSetComplete?.Invoke();
        }

        public void Reset()
        {
            BoardCells = null;
            BoardItems = null;
        }
    }
}