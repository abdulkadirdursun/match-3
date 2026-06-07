using Match3.Core.Helpers;
using UnityEngine;

namespace Match3.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Match 3/Level System/Level Database")]
    public class LevelDatabase : ScriptableObject
    {
        [SerializeField] private LevelData[] levelDataArray;

        public LevelData GetLoopLevelData(int level)
        {
            var levelIndex = level % levelDataArray.Length;
            return levelDataArray[levelIndex];
        }

        public LevelData GetRandomLevelData() => levelDataArray.Random();
    }
}