using Match3.LevelSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.HUD
{
    public class TempHUDController : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button resetLevelButton;

        private void OnNextLevelButtonClick()
        {
            levelManager.StartNewLevel();
            UpdateLevelText();
        }

        private void OnResetLevelButtonClick()
        {
            levelManager.StartTheLevel();
        }

        private void UpdateLevelText()
        {
            levelText.text = $"LEVEL {levelManager.CurrentLevel:00}";
        }

        #region MonoBehaviour Methods

        private void OnEnable()
        {
            nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            resetLevelButton.onClick.AddListener(OnResetLevelButtonClick);
        }

        private void Start()
        {
            UpdateLevelText();
        }

        private void OnDisable()
        {
            nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
            resetLevelButton.onClick.RemoveListener(OnResetLevelButtonClick);
        }

        #endregion
    }
}