using Match3.LevelSystem;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Match 3/Core/Gameplay Data")]
    public class GameplayData : ScriptableObject
    {
        [SerializeField] private LevelData levelData; //Set on inspector temporarily

        public Vector2Int BoardSize => levelData.BoardSize;
    }
}