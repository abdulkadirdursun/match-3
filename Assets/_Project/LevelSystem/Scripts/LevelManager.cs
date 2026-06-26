using Match3.Core;
using Match3.ObjectiveSystem;
using UnityEngine;

namespace Match3.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private ObjectiveService objectiveService;
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

        private void CleanTheLevel()
        {
            boardResolver.AbortTasks();
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            StartTheLevel();
        }

        #endregion
    }
}