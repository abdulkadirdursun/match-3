using UnityEngine;

namespace Match3.Core.Tiles
{
    [CreateAssetMenu(fileName = "Tile Type", menuName = "Match 3/Core/Tile Type")]
    public class TileTypeData : ScriptableObject
    {
        [SerializeField] private string tileName;
        [SerializeField] private Tile prefab;

        public string TileName => tileName;
        public Tile Prefab => prefab;
    }
}