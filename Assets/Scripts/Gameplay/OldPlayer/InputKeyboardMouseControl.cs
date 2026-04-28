using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class InputKeyboardMouseControl : IPlayerInputController
    {
        private Input m_CustomInput;

        public Vector2 MoveInput { get; private set; }
        public bool IsEnabled { get; private set; }

        public event Action OnFire;
        public event Action<Vector2, bool> OnNewDirection;
        public event Action OnMoveCancel;

        public void Init()
        {
            m_CustomInput = new Input();

            m_CustomInput.KeysGameplay.Enable();

            m_CustomInput.KeysGameplay.Move.performed += GetMoveKey;
            m_CustomInput.KeysGameplay.Move.canceled += GetMoveKey;
            m_CustomInput.KeysGameplay.Shoot.performed += OnShootClick;
            m_CustomInput.KeysGameplay.Shoot.canceled += OnShootClick;
        }

        public void Dispose()
        {
            Disable();

            if (m_CustomInput == null)
            {
                return;
            }

            m_CustomInput.KeysGameplay.Move.performed -= GetMoveKey;
            m_CustomInput.KeysGameplay.Move.canceled -= GetMoveKey;
            m_CustomInput.KeysGameplay.Shoot.performed -= OnShootClick;
            m_CustomInput.KeysGameplay.Shoot.canceled -= OnShootClick;

            //CustomInput.KeysGameplay.Disable();
            m_CustomInput.Dispose();
        }

        public void Enable()
        {
            if (IsEnabled)
            {
                return;
            }

            m_CustomInput?.KeysGameplay.Enable();
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
            {
                return;
            }

            m_CustomInput?.KeysGameplay.Disable();
            IsEnabled = false;
        }

        private void GetMoveKey(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnNewDirection?.Invoke(MoveInput, true);
        }

        private void OnShootClick(InputAction.CallbackContext ctx)
        {
            OnNewDirection?.Invoke(MoveInput, false);
            OnFire?.Invoke();
        }
    }
}