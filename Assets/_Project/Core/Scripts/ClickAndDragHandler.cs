using Match3.Core.Helpers;
using Match3.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3.Core
{
    public class ClickAndDragHandler : MonoBehaviour
    {
        [SerializeField] private GameBoardData gameBoardData;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private float checkInterval = 0.1f;

        private bool _dragging;
        private float _timePassed;
        private BoardCell _firstBoardCell;
        private float _pointerPosZ;

        private void StartItemDrag()
        {
            if (Pointer.current == null) return;
            var clickPosition = Pointer.current.position.ReadValue();
            var worldPos = mainCamera.ScreenToWorldPoint(new Vector3(clickPosition.x, clickPosition.y, _pointerPosZ));
            if (!gameBoardData.TryGetBoardCell(worldPos, out _firstBoardCell)
                || !_firstBoardCell.HasBoardItem) return;
            _dragging = true;
            _timePassed = 0f;
        }

        private void StopItemDrag()
        {
            _dragging = false;
            _firstBoardCell = null;
        }

        private bool TryGetAdjacentCell(Vector3 pointerPos, out BoardCell adjacentCell)
        {
            pointerPos.z = _pointerPosZ;
            var pointerWorldPos = mainCamera.ScreenToWorldPoint(pointerPos);
            if (gameBoardData.TryGetBoardCell(pointerWorldPos, out adjacentCell)
                && adjacentCell == _firstBoardCell) return false;

            if (adjacentCell != null)
            {
                if (_firstBoardCell.IsAdjacentTo(adjacentCell))
                    return true;

                //Get Adjacent cell at same direction
                var adjacentCoord = _firstBoardCell.GetDirection(adjacentCell) + _firstBoardCell.Coordinates;
                return gameBoardData.TryGetBoardCell(adjacentCoord, out adjacentCell);
            }

            return false;
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            _pointerPosZ = Mathf.Abs(mainCamera.transform.position.z);
        }

        private void OnEnable()
        {
            playerInputHandler.PointerDown += StartItemDrag;
            playerInputHandler.PointerUp += StopItemDrag;
        }

        private void Update()
        {
            if (!_dragging || Pointer.current == null) return;
            _timePassed += Time.deltaTime;
            if (_timePassed < checkInterval) return;
            _timePassed = 0f;
            var pointerPosition = Pointer.current.position.ReadValue();
            if (!TryGetAdjacentCell(pointerPosition, out var adjacentCell)
                || !adjacentCell.HasBoardItem) return;

            //TODO: gameBoardController.SwapItems(_firstBoardCell, adjacentCell);
            StopItemDrag();
        }

        private void OnDisable()
        {
            playerInputHandler.PointerDown -= StartItemDrag;
            playerInputHandler.PointerUp -= StopItemDrag;
        }

        #endregion
    }
};