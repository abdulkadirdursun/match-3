using Match3.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3.Core
{
    public class ClickAndDragHandler : MonoBehaviour
    {
        [SerializeField] private GameBoardController gameBoardController;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayerInputHandler playerInputHandler;
        [SerializeField] private float checkInterval = 0.1f;

        private bool _dragging;
        private float _timePassed;
        private BoardCell _firstBoardCell;
        private float _cameraDistance;

        private void StartItemDrag()
        {
            if (Pointer.current == null) return;
            var clickPosition = Pointer.current.position.ReadValue();
            var worldPos = mainCamera.ScreenToWorldPoint(new Vector3(clickPosition.x, clickPosition.y, _cameraDistance));
            if (!gameBoardController.TryToGetCellAtCoord(worldPos, out _firstBoardCell)
                || !_firstBoardCell.HasBoardItem) return;
            _dragging = true;
            _timePassed = 0f;
        }

        private void StopItemDrag()
        {
            _dragging = false;
            _firstBoardCell = null;
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            _cameraDistance -= mainCamera.transform.position.z;
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
            var worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, _cameraDistance));
            var hasCellAtPosition = gameBoardController.TryToGetCellAtCoord(worldPosition, out var hoveringCell);
            if (!hasCellAtPosition
                || hoveringCell == _firstBoardCell
                || !hoveringCell.HasBoardItem) return;

            gameBoardController.SwapItems(_firstBoardCell, hoveringCell);
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