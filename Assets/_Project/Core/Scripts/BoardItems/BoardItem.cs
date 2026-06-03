using System;
using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardItem : MonoBehaviour
    {
        [SerializeField] private Transform itemTransform;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public BoardItemData BoardItemData { get; private set; }

        public void Initialize(BoardItemData boardItemData)
        {
            BoardItemData = boardItemData;
            spriteRenderer.sprite = boardItemData.ItemSprite;
            spriteRenderer.color = boardItemData.SetRendererColor ? boardItemData.RepresentingColor : Color.white;
        }

        public Tween MoveToPos(Vector2 targetPosition, float time)
        {
            return itemTransform.DOMove(targetPosition, time);
        }

        public void Destroy() //Temp
        {
            Destroy(gameObject);
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (itemTransform == null) itemTransform = transform;
        }

        #endregion
    }
}