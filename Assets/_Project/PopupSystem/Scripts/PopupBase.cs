using UnityEngine;
using UnityEngine.UI;

namespace Match3.PopupSystem
{
    [RequireComponent(typeof(Canvas))]
    public abstract class PopupBase : MonoBehaviour
    {
        [SerializeField] private PopupService popupService;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backdrop;

        public void Open()
        {
            OnPopupOpen();
            canvas.enabled = true;
        }

        private void Close()
        {
            canvas.enabled = false;
            OnPopupClosed();
        }

        protected virtual void OnPopupOpen()
        {
        }

        protected virtual void OnPopupClosed()
        {
        }


        #region MonoBehaviour Methods

        private void OnEnable()
        {
            popupService.RegisterPopup(this);
            closeButton?.onClick.AddListener(Close);
            backdrop?.onClick.AddListener(Close);
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
        }

        #endregion
    }
}