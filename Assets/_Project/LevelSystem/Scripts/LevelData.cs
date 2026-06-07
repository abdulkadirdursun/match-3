using UnityEngine;

namespace Match3.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Match 3/Level System/Level Data")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private Vector2Int boardSize = new Vector2Int(4, 4);

        public Vector2Int BoardSize => boardSize;
    }
}