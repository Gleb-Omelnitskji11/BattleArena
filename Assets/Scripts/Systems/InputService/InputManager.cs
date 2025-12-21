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
            m_CurrentMovementSystemType = MovementSystemType.KeyboardLinear; //Todo
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            playerInputController.Subscribe();
            return playerInputController;
        }
    }

    public enum MovementSystemType
    {
        KeyboardLinear
    }
}