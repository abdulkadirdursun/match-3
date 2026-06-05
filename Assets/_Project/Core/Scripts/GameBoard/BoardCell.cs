using DG.Tweening;
using UnityEngine;

namespace Match3.Core
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private Transform cellTransform;

        public BoardItem BoardItem { get; private set; }
        public Vector2Int Coordinates { get; private set; }
        public Vector3 WorldPos => cellTransform.position;

        public bool HasBoardItem => BoardItem != null;

        public void ClearInPlace()
        {
            BoardItem.Destroy(); //Temp method call
            BoardItem = null;
        }

        public bool TryGetBoardItemData(out BoardItemData boardItemData)
        {
            if (!HasBoardItem)
            {
                Debug.Log($"<color=cyan>[BoardCell]</color> No board item on the Cell at ({Coordinates})!");
                boardItemData = null;
                return false;
            }

            boardItemData = BoardItem.BoardItemData;
            if (boardItemData == null)
                Debug.LogError($"<color=cyan>[BoardCell]</color>  Board item exist at Cell({Coordinates}) but no board item data available!!");
            return boardItemData != null;
        }

        public void Initialize(Vector3 position, Vector2Int coords, float scale)
        {
            Coordinates = coords;
            cellTransform.localScale = Vector3.one * scale;
            cellTransform.position = position;
        }

        public Tween SetItem(BoardItem boardItem)
        {
            BoardItem = boardItem;
            return BoardItem.MoveToPos(cellTransform.position, 0.2f);
        }


        #region MonoBehaviour Methods

        private void Awake()
        {
            if (!cellTransform)
            {
                Debug.LogWarning($"Set {nameof(BoardCell)} transform on inspector");
                cellTransform = transform;
            }
        }

        #endregion
    }
}