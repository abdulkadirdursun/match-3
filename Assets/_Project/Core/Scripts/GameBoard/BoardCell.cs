using Match3.Core.BoardItems;
using UnityEngine;

namespace Match3.Core.GameBoard
{
    public class BoardCell : MonoBehaviour
    {
        [SerializeField] private Transform cellTransform;

        private Vector2Int _coordinates;
        private BoardItem _boardItem;

        public void Initialize(Vector3 position, Vector2Int coords, float scale)
        {
            _coordinates = coords;
            cellTransform.localScale = Vector3.one * scale;
            cellTransform.position = position;
        }

        public void SetItem(BoardItem boardItem)
        {
            _boardItem = boardItem;
            if (_boardItem == null) return;
            _boardItem.transform.position = cellTransform.position; //Temp
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