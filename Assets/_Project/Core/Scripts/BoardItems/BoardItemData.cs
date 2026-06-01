using UnityEngine;

namespace Match3.Core.BoardItems
{
    [CreateAssetMenu(fileName = "Tile Type", menuName = "Match 3/Core/Board Item Data")]
    public class BoardItemData : ScriptableObject
    {
        [SerializeField] private string tileName;
        [SerializeField] private BoardItem prefab;

        public string TileName => tileName;
        public BoardItem Prefab => prefab;
    }
}