using System.Collections.Generic;
using UnityEngine;

namespace Match3.Core
{
    public class MatchController : MonoBehaviour
    {
        [SerializeField] private GameBoardController gameBoardController;

        public bool CheckForMatch(Vector2Int originCoord)
        {
            var originCell = gameBoardController.BoardCells[originCoord.x, originCoord.y];
            if (originCell == null || !originCell.TryGetBoardItemData(out var targetItemData)) return false;

            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up, //up
                Vector2Int.down, //down
                Vector2Int.right, //right
                Vector2Int.left //left
            };

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
                    var inBound = neighborCoord.x >= 0 && neighborCoord.x < gameBoardController.BoardSize.x &&
                                  neighborCoord.y >= 0 && neighborCoord.y < gameBoardController.BoardSize.y;
                    if (!inBound ||
                        visited.Contains(neighborCoord)) continue;
                    var neighborCell = gameBoardController.BoardCells[neighborCoord.x, neighborCoord.y];
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