using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3.InputSystem
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerActions _playerActions;
        private PlayerActions.GameplayActions _gameplayActions;

        public event Action PointerDown;
        public event Action PointerUp;

        private void Initialize()
        {
            _playerActions = new PlayerActions();
            _gameplayActions = _playerActions.Gameplay;
            _gameplayActions.Enable();
        }

        private void OnPointerDown(InputAction.CallbackContext callback)
        {
            PointerDown?.Invoke();
        }

        private void OnPointerUp(InputAction.CallbackContext callback)
        {
            PointerUp?.Invoke();
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            _gameplayActions.Click.performed += OnPointerDown;
            _gameplayActions.Click.canceled += OnPointerUp;
        }

        private void OnDisable()
        {
            _gameplayActions.Click.performed -= OnPointerDown;
            _gameplayActions.Click.canceled -= OnPointerUp;
        }

        #endregion
    }
}