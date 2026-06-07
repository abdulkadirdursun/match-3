using Match3.LevelSystem;
using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Match 3/Core/Gameplay Data")]
    public class GameplayData : ScriptableObject
    {
        private LevelData _levelData;

        public Vector2Int BoardSize => _levelData.BoardSize;

        public void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;
        }
    }
}