using System;
using Game.Player;
using TowerDefence.Core;

namespace Systems.InputService
{
    public class InputManager : IService, IInputManager
    {
        private IPlayerInputController m_CurrentPlayerInputController;
        private MovementSystemType m_CurrentMovementSystemType;

        public void Init()
        {
            m_CurrentMovementSystemType = MovementSystemType.Mobile; //Todo
        }

        public IPlayerInputController GetCurrentPlayerInputController()
        {
            return m_CurrentPlayerInputController ??= GetMovementSystem(m_CurrentMovementSystemType);
        }

        private IPlayerInputController GetMovementSystem(MovementSystemType type)
        {
            IPlayerInputController playerInputController;

            switch (type)
            {
                case MovementSystemType.KeyboardLinear:
                    playerInputController = new InputLinearControl();
                    break;
                case MovementSystemType.Mobile:
                    playerInputController = new InputMobileController();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            playerInputController.Init();
            return playerInputController;
        }
    }

    public enum MovementSystemType
    {
        KeyboardLinear,
        Mobile
    }
}