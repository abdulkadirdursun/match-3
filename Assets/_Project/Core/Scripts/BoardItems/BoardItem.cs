using System;
using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItem : MonoBehaviour
    {
        [SerializeField] private Transform itemTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private GameObject _rendererObject;

        public BoardItemData BoardItemData { get; private set; }
        public event Action<BoardItem> ObjectHid;

        public void Initialize(BoardItemData boardItemData)
        {
            BoardItemData = boardItemData;
            spriteRenderer.sprite = boardItemData.ItemSprite;
            spriteRenderer.color = boardItemData.SetRendererColor ? boardItemData.RepresentingColor : Color.white;

            Show();
        }

        public Tween MoveToPos(Vector2 targetPosition, float time, Ease ease = Ease.Linear)
        {
            return itemTransform.DOMove(targetPosition, time).SetEase(ease);
        }

        public void PlaceAt(Vector3 position)
        {
            itemTransform.position = position;
        }

        public void Show()
        {
            _rendererObject.SetActive(true);
        }

        public void Hide() //TODO: Set private when destroy/match animation function created
        {
            _rendererObject.SetActive(false);
            ObjectHid?.Invoke(this);
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (itemTransform == null) itemTransform = transform;
            _rendererObject = spriteRenderer.gameObject;
        }

        #endregion
    }
}