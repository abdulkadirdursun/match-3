using UnityEngine;

namespace Match3.Core
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private Transform cellTransform;

        public BoardItem BoardItem { get; private set; }
        public Vector2Int Coordinates { get; private set; }

        public bool HasBoardItem => BoardItem != null;

        public void ClearInPlace()
        {
            BoardItem.Destroy(); //Temp method call
        }

        public bool TryGetBoardItemData(out BoardItemData boardItemData)
        {
            if (BoardItem == null)
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

        public void SetItem(BoardItem boardItem)
        {
            this.BoardItem = boardItem;
            if (this.BoardItem == null) return;
            this.BoardItem.transform.position = cellTransform.position; //Temp
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