using System;
using Match3.Core;
using UnityEngine;

namespace Match3.ObjectiveSystem
{
    [CreateAssetMenu(fileName = "ObjectiveService", menuName = "Match 3/Objective System/Objective Service")]
    public class ObjectiveService : ScriptableObject
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