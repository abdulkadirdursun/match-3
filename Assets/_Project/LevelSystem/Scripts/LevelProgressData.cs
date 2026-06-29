using System;
using Match3.SaveSystem;
using UnityEngine;

namespace Match3.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelProgressData", menuName = "Match 3/Level System/Level Progress Data")]
    public class LevelProgressData : BaseSavedScriptableObject<LevelProgressSaveData>
    {
        public int CurrentLevel => Data.CurrentLevel;

        public event Action LevelChanged;

        public void IncreaseLevel()
        {
            Data.IncreaseLevel();
            LevelChanged?.Invoke();
        }
    }
}