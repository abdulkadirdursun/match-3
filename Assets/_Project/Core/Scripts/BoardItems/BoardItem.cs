using UnityEngine;

namespace Match3.Core
{
    public class BoardItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public BoardItemData BoardItemData { get; private set; }

        public void Initialize(BoardItemData boardItemData)
        {
            spriteRenderer.sprite = boardItemData.ItemSprite;
            spriteRenderer.color = boardItemData.SetRendererColor ? boardItemData.RepresentingColor : Color.white;
        }

        public void Destroy() //Temp
        {
            Destroy(gameObject);
        }
    }
}