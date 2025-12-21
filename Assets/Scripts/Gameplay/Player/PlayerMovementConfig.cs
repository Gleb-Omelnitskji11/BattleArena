using System;
using TowerDefence.Core;

namespace Game.Player
{
    public class PlayerMovementService : IService, IInputManager
    {
        private IPlayerInputController m_CurrentPlayerInputController;
        private MovementSystemType m_CurrentMovementSystemType;

        public void Init()
        {
            m_CurrentMovementSystemType = MovementSystemType.KeyboardLinear; //Todo
        }

        public IPlayerInputController GetCurrentPlayerInputController()
        {
            if (m_CurrentPlayerInputController == null)
                return m_CurrentPlayerInputController = GetMovementSystem(m_CurrentMovementSystemType);
            return m_CurrentPlayerInputController;
        }
        
        private IPlayerInputController GetMovementSystem(MovementSystemType type)
        {
            switch (type)
            {
                case MovementSystemType.KeyboardLinear:
                    return new InputLinearControl();

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum MovementSystemType
    {
        KeyboardLinear
    }
}