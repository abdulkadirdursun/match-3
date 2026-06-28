using Match3.PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.LevelSystem.UI
{
    public class LevelCompletePopup : PopupBase
    {
        [Header("Level Complete Popup")]
        [SerializeField] private LevelProgressData levelProgressData;
        [SerializeField] private LevelManager levelManager; //Temp reference
        [SerializeField] private TMP_Text levelLabel;
        [SerializeField] private Button continueButton;

        private void OnContinueButtonClicked()
        {
            Close();
        }

        protected override void BeforePopupOpen()
        {
            levelLabel.text = $"LEVEL {levelProgressData.CurrentLevel}";
        }

        protected override void AfterPopupClosed()
        {
            levelManager.StartNewLevel();
        }

        protected override void RegisterButtonEvents()
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        protected override void UnregisterButtonEvents()
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
    }
}