using Game.Player;
using Gameplay.Views;
using Systems.InputService;
using TowerDefence.Core;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class SeparatePlayerController : ICharacterController
    {
        private CharacterView m_CharacterView;
        private IPlayerInputController m_PlayerInputController;

        public CharacterView CharacterView => m_CharacterView;

        public void SetData(CharacterView characterView)
        {
            m_CharacterView = characterView;

            SetSystems();
        }

        ~SeparatePlayerController()
        {
            m_PlayerInputController.OnNewDirection -= OnHold;
            m_PlayerInputController.OnFire -= OnTap;
            m_PlayerInputController.OnNewDirection -= OnTouchMoved;
            m_PlayerInputController.OnMoveCancel -= OnTouchCancelled;

            m_CharacterView.OnCollision -= OnCollision;
        }

        public void ResetData()
        {
            m_CharacterView.Reset();
        }

        private void SetSystems()
        {
            if (Services.TryGet<IInputManager>(out var inputManager))
            {
                m_PlayerInputController = inputManager.GetCurrentPlayerInputController();

                if (m_PlayerInputController is InputMobileController controller)
                {
                    controller.SetPlayerObject(CharacterView.transform);
                }
            }

            m_PlayerInputController.OnNewDirection += OnHold;
            m_PlayerInputController.OnFire += OnTap;
            m_PlayerInputController.OnNewDirection += OnTouchMoved;
            m_PlayerInputController.OnMoveCancel += OnTouchCancelled;

            m_CharacterView.OnCollision += OnCollision;
        }
        

        public void Tick()
        {
        }

        private void OnHold(Vector2 pos, bool withMoving)
        {
            m_CharacterView.UpdateDirection(pos, withMoving);
        }

        private void OnTouchMoved(Vector2 pos, bool withMoving)
        {
            m_CharacterView.UpdateDirection(pos, withMoving);
        }

        private void OnTouchCancelled()
        {
            m_CharacterView.StopMove();
        }

        private void OnTap()
        {
            m_CharacterView.Attack();
        }

        private void OnCollision(GameObject obj)
        {
            m_CharacterView.StopMove();
        }
    }
}