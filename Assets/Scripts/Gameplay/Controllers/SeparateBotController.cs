using System;
using UnityEngine;

public class SeparateBotController : ICharacterController, IDisposable
{
    private readonly CharacterView m_CharacterView;
    private readonly ITargetDetector m_Detector;

    private CharacterView m_CurrentTarget;
    private Vector2 m_CurrentDirection;
    public CharacterView CharacterView => m_CharacterView;

    public SeparateBotController(CharacterView view, ITargetDetector detector)
    {
        m_CharacterView = view;
        m_Detector = detector;
        view.OnDie += OnDie;
        view.OnCollision += OnCollision;
    }

    public void Tick()
    {
        if (m_Detector.TryDetectTarget(out var target))
        {
            m_CurrentTarget = target;
            Debug.Log("Detected");
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
            UpdatePatrol();
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

    private void UpdatePatrol()
    {
        if (m_CurrentDirection == Vector2.zero)
        {
            m_CurrentDirection = GetRandomDirection();
        }

        m_CharacterView.UpdateDirection(m_CurrentDirection);
    }

    private void OnCollision(GameObject obj)
    {
        if (obj.tag.Equals(m_CharacterView.CharacterModel.TeamId.ToString()) || obj.tag.Equals(TeamId.Neutral.ToString()))
        {
            m_CurrentDirection = GetRandomDirection();
            m_CharacterView.UpdateDirection(m_CurrentDirection);
        }
    }

    private Vector2 GetRandomDirection()
    {
        int r = UnityEngine.Random.Range(0, 4);

        return r switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            _ => Vector2.zero
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
        
        m_CharacterView.OnCollision -= OnCollision;
        m_CharacterView.OnDie -= OnDie;
    }

    private void OnDie()
    {
        TeamId enemyId = m_CharacterView.GetEnemyTeamID();
        m_CharacterView.SetTeam(enemyId, false);
    }
}