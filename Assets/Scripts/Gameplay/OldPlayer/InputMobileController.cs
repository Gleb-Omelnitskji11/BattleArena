using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class InputMobileController : IPlayerInputController
    {
        private Input m_CustomInput;
        private Camera m_Camera;
        private Transform m_OriginTransform;

        public Vector2 MoveInput { get; private set; }
        public bool IsEnabled { get; private set; }

        public event Action OnFire;
        public event Action<Vector2, bool> OnNewDirection;
        public event Action OnMoveCancel;

        public void Init()
        {
            m_CustomInput = new Input();
            m_Camera = Camera.main;
            m_CustomInput.Game.Tap.performed += HandleTap;
            m_CustomInput.Game.Hold.performed += HandleHold;
            m_CustomInput.Game.TouchDelta.performed += HandleTouchMoved;
            m_CustomInput.Game.Press.canceled += HandlePressCancel;

            m_CustomInput.PCInput.Enable();
        }

        public void Dispose()
        {
            Disable();

            if (m_CustomInput == null)
            {
                return;
            }

            m_CustomInput.Game.Tap.performed -= HandleTap;
            m_CustomInput.Game.Hold.performed -= HandleHold;
            m_CustomInput.Game.TouchDelta.performed -= HandleTouchMoved;
            m_CustomInput.Game.Tap.canceled -= HandlePressCancel;
            m_CustomInput.Game.Press.canceled -= HandlePressCancel;

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

        private void HandleTap(InputAction.CallbackContext context)
        {
            if (IsPointerOverUI())
            {
                return;
            }

            Vector2 screenPos = GetTouchPosition();
            Vector2 worldPos = GetDirection(screenPos);
            OnNewDirection?.Invoke(worldPos, false);
            OnFire?.Invoke();
        }

        private void HandleHold(InputAction.CallbackContext context)
        {
            if (IsPointerOverUI())
            {
                return;
            }

            Vector2 screenPos = GetTouchPosition();
            Vector2 worldPos = GetDirection(screenPos);
            OnNewDirection?.Invoke(worldPos, true);
        }

        private void HandlePressCancel(InputAction.CallbackContext context)
        {
            OnMoveCancel?.Invoke();
        }

        private void HandleTouchMoved(InputAction.CallbackContext context)
        {
            if (IsPointerOverUI())
            {
                return;
            }

            var delta = context.ReadValue<Vector2>();

            if (delta.magnitude > 0.1f)
            {
                Vector2 screenPos = GetTouchPosition();
                Vector2 worldPos = GetDirection(screenPos);
                OnNewDirection?.Invoke(worldPos, false);
            }
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null || m_CustomInput == null)
            {
                return false;
            }

            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = GetTouchPosition()
            };

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            return results.Count > 0;
        }

        private Vector2 GetTouchPosition()
        {
            return m_CustomInput?.Game.TouchPosition.ReadValue<Vector2>() ?? Vector2.zero;
        }

        private Vector2 GetDirection(Vector2 screenPos)
        {
            Vector3 worldPos = m_Camera.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Mathf.Abs(m_Camera.transform.position.z)));

            Vector2 dir = new(worldPos.x - m_OriginTransform.position.x,
                worldPos.y - m_OriginTransform.position.y);

            return dir;
        }
    }
}