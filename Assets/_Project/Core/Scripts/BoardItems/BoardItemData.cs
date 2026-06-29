using UnityEngine;

namespace Match3.Core
{
    [CreateAssetMenu(fileName = "BoardItemData", menuName = "Match 3/Core/Board Item Data")]
    public class BoardItemData : ScriptableObject
    {
        [SerializeField] private string tileName;
        [SerializeField] private Sprite itemSprite;

        public string TileName => tileName;
        public Sprite ItemSprite => itemSprite;
    }
}