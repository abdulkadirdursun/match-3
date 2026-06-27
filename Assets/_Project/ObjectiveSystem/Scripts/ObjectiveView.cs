using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.ObjectiveSystem
{
    public class ObjectiveView : MonoBehaviour
    {
        [SerializeField] private ObjectiveService objectiveService;
        [SerializeField] private Image objectiveImage;
        [SerializeField] private TMP_Text objectiveAmountLabel;

        private void UpdateView()
        {
            if (objectiveService.TargetItemData == null) return;
            objectiveImage.sprite = objectiveService.TargetItemData.ItemSprite;
            objectiveAmountLabel.text = $"x{objectiveService.RemainingTargetAmount}";
        }

        #region MonoBehaviour Methods

        private void OnEnable()
        {
            objectiveService.ObjectiveChanged += UpdateView;
        }

        private void Start()
        {
            UpdateView();
        }

        private void OnDisable()
        {
            objectiveService.ObjectiveChanged -= UpdateView;
        }

        #endregion
    }
}