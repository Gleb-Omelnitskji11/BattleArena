using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class InputMobileController : IPlayerInputController, IDisposable
    {
        private Input CustomInput;

        private Vector2 m_MoveInput;
        private bool m_ShootPressed;

        Vector2 IPlayerInputController.MoveInput
        {
            get => m_MoveInput;
        }

        bool IPlayerInputController.ShootPressed
        {
            get => m_ShootPressed;
        }

        public void Init()
        {
            CustomInput = new Input();

            CustomInput.Game.Enable();

            CustomInput.Game.TouchDelta.performed += OnMove;
            CustomInput.Game.TouchDelta.canceled += OnMove;
            CustomInput.Game.Tap.performed += OnShoot;
            CustomInput.Game.Tap.canceled += OnShoot;
        }

        public void Dispose()
        {
            CustomInput.Game.TouchDelta.performed -= OnMove;
            CustomInput.Game.TouchDelta.canceled -= OnMove;
            CustomInput.Game.Tap.performed -= OnShoot;
            CustomInput.Game.Tap.canceled -= OnShoot;

            CustomInput.Game.Disable();
        }

        protected void OnMove(InputAction.CallbackContext ctx)
        {
            m_MoveInput = ctx.ReadValue<Vector2>();

            if (m_MoveInput.sqrMagnitude > 1) m_MoveInput.y = 0;
        }

        protected void OnShoot(InputAction.CallbackContext ctx)
        {
            m_ShootPressed = ctx.ReadValueAsButton();
        }
    }
}
