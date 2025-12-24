using UnityEngine;

public class TankMovementController : IMovementController
{
    private readonly Transform m_Origin;
    private readonly MovementStats m_Stats;
    private Camera m_Camera;
    private bool m_CanMove;

    public Vector2 MoveDirection { get; private set; }

    public TankMovementController(Transform origin, MovementStats stats)
    {
        m_Origin = origin;
        m_Stats = stats;
        m_Camera = Camera.main;
    }

    // public void SetDirection(Vector2 direction, bool canMove = true)
    // {
    //     if (direction == Vector2.zero)
    //     {
    //         MoveDirection = Vector2.zero;
    //         m_CanMove = false;
    //         return;
    //     }
    //     
    //     m_CanMove = canMove;
    //     MoveDirection = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
    //         ? new Vector2(Mathf.Sign(direction.x), 0)
    //         : new Vector2(0, Mathf.Sign(direction.y));
    //
    //     RotateToDirection(MoveDirection);
    // }
    
    public void SetDirection(Vector2 screenPos, bool canMove = true)
    {
        Vector3 worldPos = m_Camera.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, Mathf.Abs(m_Camera.transform.position.z))
        );
    
        Vector2 delta = worldPos - m_Origin.position;
    
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
                SetDirection(Direction.Right);
            else
                SetDirection(Direction.Left);
        }
        else
        {
            if (delta.y > 0)
                SetDirection(Direction.Up);
            else
                SetDirection(Direction.Down);
        }
        
        m_CanMove = canMove;
    }
    
    private void SetDirection(Direction direction)
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

    // private void RotateToDirection(Vector2 dir)
    // {
    //     float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
    //     m_Origin.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    // }
}