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

            m_CustomInput.PCInput.Enable();

            m_CustomInput.PCInput.Move.performed += GetMoveKey;
            m_CustomInput.PCInput.Move.canceled += GetMoveKey;
            m_CustomInput.PCInput.Shoot.performed += OnShootClick;
            m_CustomInput.PCInput.Shoot.canceled += OnShootClick;
        }

        public void Dispose()
        {
            Disable();

            if (m_CustomInput == null)
            {
                return;
            }

            m_CustomInput.PCInput.Move.performed -= GetMoveKey;
            m_CustomInput.PCInput.Move.canceled -= GetMoveKey;
            m_CustomInput.PCInput.Shoot.performed -= OnShootClick;
            m_CustomInput.PCInput.Shoot.canceled -= OnShootClick;

            //CustomInput.PCInput.Disable();
            m_CustomInput.Dispose();
        }

        public void Enable()
        {
            if (IsEnabled)
            {
                return;
            }

            m_CustomInput?.Game.Enable();
            IsEnabled = true;
        }

        public void Disable()
        {
            if (!IsEnabled)
            {
                return;
            }

            m_CustomInput?.Game.Disable();
            IsEnabled = false;
        }

        private void GetMoveKey(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnNewDirection?.Invoke(MoveInput, true);
        }

        private void OnShootClick(InputAction.CallbackContext ctx)
        {
            MoveInput = ctx.ReadValue<Vector2>();
            OnNewDirection?.Invoke(MoveInput, false);
            OnFire?.Invoke();
        }
    }
}