using System;
using UnityEngine;

public class SeparateBotController : ICharacterController, IDisposable
{
    private readonly CharacterView m_CharacterView;
    private CharacterModel m_Model;
    private readonly ITargetDetector m_Detector;

    private CharacterView m_CurrentTarget;
    private Vector2 m_CurrentDirection;
    public CharacterView CharacterView => m_CharacterView;

    public SeparateBotController(CharacterView view, ITargetDetector detector)
    {
        m_CharacterView = view;
        m_Detector = detector;
        view.OnDie += OnDie;
    }

    public void Tick()
    {
        if (m_Detector.TryDetectTarget(out var target))
        {
            m_CurrentTarget = target;
        }

        if (m_CurrentTarget != null)
        {
            Vector2 dir = (m_CurrentTarget.transform.position - m_CharacterView.transform.position).normalized;

            dir = ClampToCardinal(dir);

            m_CharacterView.UpdateDirection(dir);

            if (IsTargetInFront(m_CharacterView.transform, m_CurrentDirection, m_CurrentTarget.transform))
            {
                m_CharacterView.Attack();
            }
        }
        else
        {
            Patrol();
        }
    }

    private bool IsTargetInFront(Transform bot, Vector2 botForward, Transform target)
    {
        if (botForward == Vector2.zero)
            return false;

        Vector2 toTarget = ((Vector2)target.position - (Vector2)bot.position).normalized;

        float dot = Vector2.Dot(botForward, toTarget);
        float cosThreshold = Mathf.Cos(0.9f * 0.5f * Mathf.Deg2Rad);

        return dot >= cosThreshold;
    }

    private void Patrol()
    {
        if (m_CurrentDirection == Vector2.zero)
        {
            m_CurrentDirection = GetRandomDirection();
        }

        m_CharacterView.UpdateDirection(m_CurrentDirection);
    }

    private Vector2 GetRandomDirection()
    {
        int r = UnityEngine.Random.Range(0, 4);

        return r switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            _ => Vector2.right
        };
    }

    private Vector2 ClampToCardinal(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);

        return new Vector2(0, Mathf.Sign(dir.y));
    }

    public void Dispose()
    {
        m_CharacterView.StopMove();
    }

    private void OnDie()
    {
        TeamId enemyId = m_CharacterView.GetEnemyTeamID();
        m_CharacterView.SetTeam(enemyId, false);
    }
}