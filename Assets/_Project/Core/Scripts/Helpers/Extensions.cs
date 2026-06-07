using UnityEngine;

namespace Match3.Core.Helpers
{
    public static class Extensions
    {
        public static bool IsAdjacentTo(this Vector2Int origin, Vector2Int target)
        {
            var diff = target - origin;

            return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
        }

        public static bool IsAdjacentTo(this BoardCell origin, BoardCell target)
        {
            var diff = target.Coordinates - origin.Coordinates;

            return Mathf.Abs(diff.x) + Mathf.Abs(diff.y) == 1;
        }

        public static Vector2Int GetDirection(this BoardCell origin, BoardCell target)
        {
            var diff = target.Coordinates - origin.Coordinates;
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                return diff.x < 0 ? Vector2Int.left : Vector2Int.right;

            return diff.y < 0 ? Vector2Int.down : Vector2Int.up;
        }
    }
}