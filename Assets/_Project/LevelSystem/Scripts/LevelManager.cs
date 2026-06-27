using Match3.Core;
using Match3.LevelSystem.UI;
using Match3.ObjectiveSystem;
using Match3.PopupSystem;
using UnityEngine;

namespace Match3.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private ObjectiveService objectiveService;
        [SerializeField] private PopupService popupService;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private GameBoardCreator gameBoardCreator;
        [SerializeField] private BoardItemSpawner boardItemSpawner;
        [SerializeField] private BoardResolver boardResolver;

        public int CurrentLevel { get; private set; } = 1; //Temp variable

        public void StartTheLevel()
        {
            CleanTheLevel();
            var levelData = levelDatabase.GetLoopLevelData(CurrentLevel);
            objectiveService.StartObjectiveProgress(levelData.TargetItemData, levelData.TargetAmount);
            gameBoardData.SetBoardSize(levelData.BoardSize);
            gameBoardCreator.GenerateBoard();
            boardItemSpawner.SpawnBoardItems();
        }

        public void StartNewLevel()
        {
            CurrentLevel++;
            StartTheLevel();
        }

        private void OnLevelComplete()
        {
            popupService.ShowPopup<LevelCompletePopup>();
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