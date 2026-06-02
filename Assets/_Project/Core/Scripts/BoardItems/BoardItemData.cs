using UnityEngine;

namespace Match3.Core.BoardItems
{
    [CreateAssetMenu(fileName = "BoardItemData", menuName = "Match 3/Core/Board Item Data")]
    public class BoardItemData : ScriptableObject
    {
        [SerializeField] private string tileName;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private Color representingColor = Color.white; // For level editor use
        [SerializeField] private bool setRendererColor = true; //Temp value until item sprites imported

        public string TileName => tileName;
        public Sprite ItemSprite => itemSprite;
        public Color RepresentingColor => representingColor;
        public bool SetRendererColor => setRendererColor;
    }
}