using UnityEngine;

namespace Match3.Core.Tiles
{
    [CreateAssetMenu(fileName = "Tile Type", menuName = "Match 3/Core/Tile Type")]
    public class BoadItemData : ScriptableObject
    {
        [SerializeField] private string tileName;
        [SerializeField] private BoardItem prefab;

        public string TileName => tileName;
        public BoardItem Prefab => prefab;
    }
}