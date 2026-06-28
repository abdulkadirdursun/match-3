using System;
using UnityEngine;

namespace Match3.LevelSystem
{
    [Serializable]
    public class LevelProgressSaveData
    {
        [SerializeField] private int currentLevel = 1;

        public int CurrentLevel => currentLevel;

        public void IncreaseLevel() => currentLevel++;
    }
}