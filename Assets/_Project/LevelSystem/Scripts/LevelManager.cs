using Match3.Core;
using Match3.GameZone;
using Match3.LevelSystem.UI;
using Match3.ObjectiveSystem;
using Match3.PopupSystem;
using UnityEngine;

namespace Match3.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelProgressData levelProgressData;
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private ObjectiveService objectiveService;
        [SerializeField] private PopupService popupService;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private GameBoardCreator gameBoardCreator;
        [SerializeField] private BoardItemSpawner boardItemSpawner;
        [SerializeField] private BoardResolver boardResolver;
        [SerializeField] private GameZoneManager gameZoneManager;

        public void StartTheLevel()
        {
            CleanTheLevel();
            var levelData = levelDatabase.GetLoopLevelData(levelProgressData.CurrentLevel);
            objectiveService.StartObjectiveProgress(levelData.TargetItemData, levelData.TargetAmount);
            gameBoardData.SetBoardSize(levelData.BoardSize);
            gameZoneManager.Initialize();
            gameBoardCreator.GenerateBoard();
            boardItemSpawner.SpawnBoardItems();
        }

        private void StartNewLevel()
        {
            levelProgressData.IncreaseLevel();
            StartTheLevel();
        }

        private void OnLevelComplete()
        {
            popupService.ShowPopup<LevelCompletePopup>(StartNewLevel);
        }

        private void CleanTheLevel()
        {
            boardResolver.AbortTasks();
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            StartTheLevel();
        }

        private void OnEnable()
        {
            objectiveService.ObjectiveComplete += OnLevelComplete;
        }

        private void OnDisable()
        {
            objectiveService.ObjectiveComplete -= OnLevelComplete;
        }

        #endregion
    }
}