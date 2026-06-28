using Match3.LevelSystem;
using TMPro;
using UnityEngine;

namespace Match3.HUD
{
    public class TempHUDController : MonoBehaviour
    {
        [SerializeField] private LevelProgressData levelProgressData;
        [SerializeField] private TMP_Text levelText;

        private void UpdateLevelText()
        {
            levelText.text = $"LEVEL {levelProgressData.CurrentLevel:00}";
        }

        #region MonoBehaviour Methods

        private void OnEnable()
        {
            levelProgressData.LevelChanged += UpdateLevelText;
        }

        private void Start()
        {
            UpdateLevelText();
        }

        private void OnDisable()
        {
            levelProgressData.LevelChanged -= UpdateLevelText;
        }

        #endregion
    }
}