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

            var orthographicSizeByHeight = boardSize.y * 0.5f + (2f * gameZoneConfig.BoardAreaPadding);
            var orthographicSizeByWidth = (boardSize.x * 0.5f) / mainCamera.aspect + (2f * gameZoneConfig.BoardAreaPadding);

            var orthographicSize = Mathf.Max(orthographicSizeByHeight, orthographicSizeByWidth, gameZoneConfig.MinOrthographicSize);
            mainCamera.orthographicSize = orthographicSize;

            var worldHeight = orthographicSize * 2f;
            var worldWidth = worldHeight * mainCamera.aspect;
            backgroundSpriteRenderer.size = new Vector2(worldWidth, worldHeight);
        }
    }
}