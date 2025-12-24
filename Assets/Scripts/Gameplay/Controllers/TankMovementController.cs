using UnityEngine;

public class TankMovementController : IMovementController
{
    private readonly Transform m_Origin;
    private readonly MovementStats m_Stats;
    private bool m_CanMove;

    public Vector2 Direction { get; private set; }

    public TankMovementController(Transform origin, MovementStats stats)
    {
        m_Origin = origin;
        m_Stats = stats;
    }

    public void SetDirection(Vector2 direction, bool canMove = true)
    {
        if (direction == Vector2.zero)
        {
            Direction = Vector2.zero;
            m_CanMove = false;
            return;
        }
        
        m_CanMove = canMove;
        Direction = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? new Vector2(Mathf.Sign(direction.x), 0)
            : new Vector2(0, Mathf.Sign(direction.y));

        RotateToDirection(Direction);
    }

    public void Move(float deltaTime)
    {
        if (m_CanMove || Direction == Vector2.zero)
            return;

        m_Origin.position += (Vector3)(Direction * m_Stats.Speed * deltaTime);
    }

    public void Stop()
    {
        m_CanMove = false;
        Direction = Vector2.zero;
    }

    private void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        m_Origin.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
}