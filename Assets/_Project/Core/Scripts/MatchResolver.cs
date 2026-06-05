using System.Collections.Generic;
using UnityEngine;

namespace Match3.Core
{
    public class MatchResolver : MonoBehaviour
    {
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private GameplayData gameplayData;

        private static readonly Vector2Int[] Directions = new[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.left
        };

        public bool HasMatchAt(Vector2Int coord)
        {
            if (!gameBoardData.TryGetBoardCell(coord, out var originCell)
                || !originCell.HasBoardItem) return false;
            var matchingBlob = FloodFillBlob(originCell);
            return IsMatchValid(matchingBlob);
        }

        public HashSet<BoardCell> FindAllMatches()
        {
            var result = new HashSet<BoardCell>();
            var visited = new HashSet<Vector2Int>();
            var boardSize = gameplayData.LevelData.BoardSize;
            for (int y = 0; y < boardSize.y; y++)
            {
                for (int x = 0; x < boardSize.x; x++)
                {
                    var coord = new Vector2Int(x, y);
                    if (visited.Contains(coord)
                        || !gameBoardData.TryGetBoardCell(coord, out var cell)
                        || !cell.HasBoardItem) continue;

                    var blob = FloodFillBlob(cell);
                    if (!IsMatchValid(blob))
                    {
                        foreach (var blobCell in blob) visited.Add(blobCell.Coordinates);
                        continue;
                    }

                    foreach (var blobCell in blob)
                    {
                        visited.Add(blobCell.Coordinates);
                        result.Add(blobCell);
                    }
                }
            }

            return result;
        }

        private HashSet<BoardCell> FloodFillBlob(BoardCell originCell)
        {
            originCell.TryGetBoardItemData(out var targetItemData);

            var boardSize = gameplayData.LevelData.BoardSize;

            var blob = new HashSet<BoardCell>();
            var visited = new HashSet<Vector2Int>();
            var lookUp = new Queue<BoardCell>();
            lookUp.Enqueue(originCell);
            visited.Add(originCell.Coordinates);

            while (lookUp.Count > 0)
            {
                var currentCell = lookUp.Dequeue();
                blob.Add(currentCell);
                for (int i = 0; i < Directions.Length; i++)
                {
                    var neighborCoord = currentCell.Coordinates + Directions[i];
                    if (visited.Contains(neighborCoord)
                        || !gameBoardData.TryGetBoardCell(neighborCoord, out var neighborCell)
                        || !neighborCell.TryGetBoardItemData(out var neighborItemData)
                        || neighborItemData != targetItemData) continue;

                    visited.Add(neighborCoord);
                    lookUp.Enqueue(neighborCell);
                }
            }

            return blob;
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