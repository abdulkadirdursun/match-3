using System;
using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItem : MonoBehaviour
    {
        [SerializeField] private Transform itemTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoardAnimationConfig boardAnimationConfig;

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

        public Tween MoveToPos(Vector2 targetPosition)
        {
            itemTransform.DOKill();
            return itemTransform.DOMove(targetPosition, boardAnimationConfig.SwapDuration)
                .SetEase(boardAnimationConfig.SwapEase);
        }

        public Tween FallToPos(Vector2 targetPosition)
        {
            itemTransform.DOKill();
            return itemTransform.DOMove(targetPosition, boardAnimationConfig.FallDuration)
                .SetEase(boardAnimationConfig.FallEase);
        }

        public Tween FallToPos(Vector2 targetPosition, float delay)
        {
            return FallToPos(targetPosition)
                .SetDelay(delay);
        }

        public Sequence BounceToAndBack(Vector2 awayPosition, Vector2 homePosition)
        {
            itemTransform.DOKill();
            var timePerMovement = boardAnimationConfig.BounceDuration * 0.5f;
            var seq = DOTween.Sequence();
            seq.Append(itemTransform.DOMove(awayPosition, timePerMovement)
                .SetEase(boardAnimationConfig.BounceEase));
            seq.Append(itemTransform.DOMove(homePosition, timePerMovement)
                .SetEase(boardAnimationConfig.BounceEase));
            return seq;
        }

        public Tween HideRequest()
        {
            itemTransform.DOKill();
            return itemTransform.DOScale(Vector3.zero, boardAnimationConfig.HideDuration)
                .SetEase(boardAnimationConfig.HideEase)
                .OnComplete(Hide);
        }

        public void PlaceAt(Vector3 position)
        {
            itemTransform.position = position;
        }

        public void Reset()
        {
            itemTransform.DOKill();
            _rendererObject.SetActive(false);
            transform.localScale = Vector3.one;
        }

        private void Show()
        {
            _rendererObject.SetActive(true);
        }

        private void Hide()
        {
            Reset();
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