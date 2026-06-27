using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.PopupSystem
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField] private PopupService popupService;
        [SerializeField] private PopupAnimationConfig popupAnimationConfig;
        [Header("Components")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform contentRootTransform;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backdrop;

        public void Open()
        {
            BeforePopupOpen();
            canvasGroup.interactable = false;
            canvas.enabled = true;
            contentRootTransform.localScale = Vector3.one * popupAnimationConfig.StartScaleAtOpen;
            contentRootTransform.DOScale(1f, popupAnimationConfig.OpenTime)
                .SetEase(popupAnimationConfig.OpenEase)
                .OnComplete(() =>
                {
                    canvasGroup.interactable = true;
                });
        }

        protected void Close()
        {
            canvasGroup.interactable = false;
            contentRootTransform.DOScale(popupAnimationConfig.TargetScaleAtClose, popupAnimationConfig.CloseTime)
                .SetEase(popupAnimationConfig.CloseEase)
                .OnComplete(() =>
                {
                    canvas.enabled = false;
                    AfterPopupClosed();
                });
        }

        protected virtual void BeforePopupOpen()
        {
        }

        protected virtual void AfterPopupClosed()
        {
        }

        protected virtual void RegisterButtonEvents()
        {
        }

        protected virtual void UnregisterButtonEvents()
        {
        }


        #region MonoBehaviour Methods

        private void OnEnable()
        {
            popupService.RegisterPopup(this);
            closeButton?.onClick.AddListener(Close);
            backdrop?.onClick.AddListener(Close);
            RegisterButtonEvents();
        }

        private void Start()
        {
            Close();
        }

        private void OnDisable()
        {
            popupService.UnregisterPopup(this);
            closeButton?.onClick.RemoveListener(Close);
            backdrop?.onClick.RemoveListener(Close);
            UnregisterButtonEvents();
        }

        #endregion
    }
}