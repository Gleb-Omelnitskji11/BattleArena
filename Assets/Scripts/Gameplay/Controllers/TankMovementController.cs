using Gameplay.Models;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class TankMovementController : IMovementController
    {
        private readonly Transform m_Origin;
        private readonly MovementStats m_Stats;
        private bool m_CanMove;

        public Vector2 MoveDirection { get; private set; }

        public TankMovementController(Transform origin, MovementStats stats)
        {
            m_Origin = origin;
            m_Stats = stats;
        }
    
        public void SetDirection(Vector2 direction, bool canMove = true)
        {
            if (direction == Vector2.zero) return;
    
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    ChooseDirection(Direction.Right);
                else
                    ChooseDirection(Direction.Left);
            }
            else
            {
                if (direction.y > 0)
                    ChooseDirection(Direction.Up);
                else
                    ChooseDirection(Direction.Down);
            }
        
            m_CanMove = canMove;
        }
    
        private void ChooseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    MoveDirection = Vector2.up;
                    m_Origin.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case Direction.Down:
                    MoveDirection = Vector2.down;
                    m_Origin.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Direction.Left:
                    MoveDirection = Vector2.left;
                    m_Origin.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case Direction.Right:
                    MoveDirection = Vector2.right;
                    m_Origin.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
        }

        public void Move(float deltaTime)
        {
            if (!m_CanMove || MoveDirection == Vector2.zero)
                return;

            m_Origin.position += (Vector3)(MoveDirection * m_Stats.Speed * deltaTime);
        }

        public void Stop()
        {
            m_CanMove = false;
            MoveDirection = Vector2.zero;
        }
    }
}