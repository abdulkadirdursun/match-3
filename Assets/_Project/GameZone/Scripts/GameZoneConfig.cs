using UnityEngine;

namespace Match3.GameZone
{
    [CreateAssetMenu(fileName = "GameZoneConfig", menuName = "Match 3/Game Zone/Game Zone Config")]
    public class GameZoneConfig : ScriptableObject
    {
        [SerializeField, Tooltip("Extra world-unit margin around the board")] private float boardAreaPadding = 0.5f;
        [SerializeField, Tooltip("Min value orthographic camera size can get")] private float minOrthographicSize = 5f;

        public float BoardAreaPadding => boardAreaPadding;
        public float MinOrthographicSize => minOrthographicSize;
    }
}