using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "Board Config", menuName = "Match 3/Core/Board Config")]
    public class BoardConfig : ScriptableObject
    {
        [SerializeField] private float cellSize = 1f;

        public float CellSize => cellSize;
    }
}