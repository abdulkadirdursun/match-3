using Match3.Core;
using UnityEngine;

namespace Match3.GameZone
{
    public class GameZoneManager : MonoBehaviour
    {
        [SerializeField] private GameZoneConfig gameZoneConfig;
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private BoardConfig boardConfig;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        public void Initialize()
        {
            Vector2 boardSize = new Vector2(gameBoardData.BoardSize.x, gameBoardData.BoardSize.y) * boardConfig.CellSize;

            var cameraSizeByHeight = boardSize.y * 0.5f + (2f * gameZoneConfig.BoardAreaPadding);
            var cameraSizeByWidth = (boardSize.x * 0.5f) / mainCamera.aspect + (2f * gameZoneConfig.BoardAreaPadding);

            mainCamera.orthographicSize = Mathf.Max(cameraSizeByHeight, cameraSizeByWidth, gameZoneConfig.MinOrthographicSize);
        }
    }
}