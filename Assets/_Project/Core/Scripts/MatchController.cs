using System.Collections.Generic;
using UnityEngine;

namespace Match3.Core
{
    public class MatchController : MonoBehaviour
    {
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private GameplayData gameplayData;

        public bool CheckForMatch(Vector2Int originCoord)
        {
            if (!gameBoardData.TryGetBoardCell(originCoord, out var originCell)) return false;
            if (!originCell || !originCell.TryGetBoardItemData(out var targetItemData)) return false;

            Vector2Int[] directions =
            {
                Vector2Int.up, //up
                Vector2Int.down, //down
                Vector2Int.right, //right
                Vector2Int.left //left
            };
            
            var boardSize = gameplayData.LevelData.BoardSize;

            var matches = new HashSet<BoardCell>();
            var visited = new HashSet<Vector2Int>();
            var lookUp = new Queue<BoardCell>();
            lookUp.Enqueue(originCell);

            while (lookUp.Count > 0)
            {
                var currentCell = lookUp.Dequeue();
                matches.Add(currentCell);
                for (int i = 0; i < directions.Length; i++)
                {
                    var neighborCoord = currentCell.Coordinates + directions[i];
                    var inBound = neighborCoord.x >= 0 && neighborCoord.x < boardSize.x &&
                                  neighborCoord.y >= 0 && neighborCoord.y < boardSize.y;
                    if (!inBound
                        || visited.Contains(neighborCoord)
                        || !gameBoardData.TryGetBoardCell(neighborCoord, out var neighborCell)) continue;
                    if (!neighborCell.TryGetBoardItemData(out var boardItemData) || boardItemData != targetItemData) continue;
                    visited.Add(neighborCoord);
                    lookUp.Enqueue(neighborCell);
                }
            }

            if (!IsMatchValid(matches)) return false;

            foreach (var matchedCell in matches)
            {
                matchedCell.ClearInPlace();
            }

            return true;
        }

        private bool IsMatchValid(HashSet<BoardCell> matchedCells)
        {
            var coords = new HashSet<Vector2Int>();
            foreach (var cells in matchedCells) coords.Add(cells.Coordinates);
            //ContainsStraightLine
            foreach (var coord in coords)
            {
                if (coords.Contains(coord + Vector2Int.left) && coords.Contains(coord + Vector2Int.right)
                    || coords.Contains(coord + Vector2Int.up) && coords.Contains(coord + Vector2Int.down))
                    return true;
            }

            return false;
        }
    }
}