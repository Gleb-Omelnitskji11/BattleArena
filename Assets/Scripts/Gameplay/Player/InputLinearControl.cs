using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class InputLinearControl : IPlayerInputController, IDisposable
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

            CustomInput.KeysGameplay.Enable();

            CustomInput.KeysGameplay.Move.performed += OnMove;
            CustomInput.KeysGameplay.Move.canceled += OnMove;
            CustomInput.KeysGameplay.Shoot.performed += OnShoot;
            CustomInput.KeysGameplay.Shoot.canceled += OnShoot;
        }

        public void Dispose()
        {
            CustomInput.KeysGameplay.Move.performed -= OnMove;
            CustomInput.KeysGameplay.Move.canceled -= OnMove;
            CustomInput.KeysGameplay.Shoot.performed -= OnShoot;
            CustomInput.KeysGameplay.Shoot.canceled -= OnShoot;

            CustomInput.KeysGameplay.Disable();
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