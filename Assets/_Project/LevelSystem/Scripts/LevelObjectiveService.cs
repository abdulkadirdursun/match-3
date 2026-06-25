using System;
using Match3.Core;
using UnityEngine;

namespace Match3.LevelSystem
{
    [CreateAssetMenu(fileName = "LevelObjectiveService", menuName = "Match 3/Level System/Level Objective Service")]
    public class LevelObjectiveService : ScriptableObject
    {
        public BoardItemData TargetItemData { get; private set; }
        public int RemainingTargetAmount { get; private set; }

        private bool _targetReached;

        public event Action LevelObjectiveComplete;

        public void StartObjectiveProgress(BoardItemData targetItemData, int targetAmount)
        {
            TargetItemData = targetItemData;
            RemainingTargetAmount = targetAmount;
            _targetReached = false;
        }

        public void ItemBroke(BoardItemData itemData)
        {
            if (_targetReached || itemData != TargetItemData) return;
            RemainingTargetAmount--;
            if (RemainingTargetAmount > 0) return;
            _targetReached = true;
            LevelObjectiveComplete?.Invoke();
        }
    }
}