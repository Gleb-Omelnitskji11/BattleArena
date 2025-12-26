using System;
using Gameplay.Views;
using TowerDefence.Core;
using TowerDefence.Game;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class SeparatePlayerController : ICharacterController
    {
        private CharacterView m_CharacterView;
        private IInputService m_PlayerInputController;
        private IEventBus m_EventBus;
        private Camera m_Camera;

        public CharacterView CharacterView => m_CharacterView;

        public void SetData(CharacterView characterView)
        {
            m_CharacterView = characterView;
            m_Camera = Camera.main;

            SetSystems();
        }

        ~SeparatePlayerController()
        {
            m_PlayerInputController.OnHold -= OnHold;
            m_PlayerInputController.OnTap -= OnTap;
            m_PlayerInputController.OnTouchMoved -= OnTouchMoved;
            m_PlayerInputController.OnCancel -= OnTouchCancelled;

            m_CharacterView.OnCollision -= OnCollision;
            m_CharacterView.OnDie -= OnDie;
        }

        public void ResetData()
        {
            m_CharacterView.Reset();
        }

        private void SetSystems()
        {
            m_EventBus = Services.Get<IEventBus>();

            if (Services.TryGet<IInputService>(out var inputService))
            {
                m_PlayerInputController = inputService;
            }

            m_PlayerInputController.OnHold += OnHold;
            m_PlayerInputController.OnTap += OnTap;
            m_PlayerInputController.OnTouchMoved += OnTouchMoved;
            m_PlayerInputController.OnCancel += OnTouchCancelled;

            m_CharacterView.OnCollision += OnCollision;
            m_CharacterView.OnDie += OnDie;
        }

        public void Tick()
        {
        }

        private void OnHold(Vector2 screenPos)
        {
            Vector2 worldPos = GetDirection(screenPos);
            m_CharacterView.UpdateDirection(worldPos);
        }

        private void OnTouchMoved(Vector2 screenPos)
        {
            Vector2 worldPos = GetDirection(screenPos);
            m_CharacterView.UpdateDirection(worldPos);
        }

        private void OnTouchCancelled()
        {
            m_CharacterView.StopMove();
        }

        private void OnTap(Vector2 screenPos)
        {
            Vector2 worldPos = GetDirection(screenPos);
            m_CharacterView.UpdateDirection(worldPos, false);
            m_CharacterView.Attack();
        }

        private Vector2 GetDirection(Vector2 screenPos)
        {
            Vector3 worldPos = m_Camera.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Mathf.Abs(m_Camera.transform.position.z)));

            Vector2 dir = new(worldPos.x - m_CharacterView.transform.position.x,
                worldPos.y - m_CharacterView.transform.position.y);

            return dir;
        }

        private void OnCollision(GameObject obj)
        {
            m_CharacterView.StopMove();
        }

        private void OnDie()
        {
            Debug.Log("You are Dead");
            m_EventBus.Publish(new GameOverEvent());
        }
    }
}