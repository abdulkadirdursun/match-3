using UnityEngine;

namespace Match3.Core.BoardItems
{
    public class BoardItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private BoardItemData _boardItemData;

        public void Initialize(BoardItemData boardItemData)
        {
            spriteRenderer.sprite = boardItemData.ItemSprite;
            spriteRenderer.color = boardItemData.SetRendererColor ? boardItemData.RepresentingColor : Color.white;
        }
    }
}