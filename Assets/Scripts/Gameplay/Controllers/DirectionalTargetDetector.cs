using System.Collections.Generic;
using UnityEngine;

public class DirectionalTargetDetector : ITargetDetector
{
    private readonly Transform m_Origin;
    private readonly CharacterView m_View;

    private const float Radius = 30f;
    private const float ViewAngle = 30f;

    private readonly List<CharacterView> AllEnemies = new List<CharacterView>();
    private CharacterModel CharacterModel => m_View.CharacterModel;
    private LevelProgressChecker m_LevelProgressChecker;
    

    public DirectionalTargetDetector(Transform origin, CharacterView characterView, LevelProgressChecker levelProgressChecker)
    {
        m_Origin = origin;
        m_View = characterView;
        m_LevelProgressChecker = levelProgressChecker;
    }

    public void UpdateEnemies()
    {
        foreach (var character in m_LevelProgressChecker.AllBots)
        {
            if (!IsEnemy(character))
                continue;

            AllEnemies.Add(character);
        }
    }

    public bool TryDetectTarget(out CharacterView target)
    {
        target = null;

        if (m_View.Direction == Vector2.zero)
            return false;

        foreach (var character in AllEnemies)
        {
            if (!IsInFront(character))
                continue;

            if (!HasLineOfSight(character))
                continue;

            if (target)
            {
                //if character further then target
                if ((target.transform.position - m_Origin.position).magnitude
                    <= (character.transform.position - m_Origin.position).magnitude)
                    continue;
            }

            target = character;
            return true;
        }

        return false;
    }

    private bool IsEnemy(CharacterView candidate)
    {
        return candidate.TeamId != CharacterModel.TeamId && candidate.TeamId != TeamId.Neutral;
    }

    private bool IsIt(CharacterView candidate)
    {
        return candidate.transform == m_Origin;
    }

    private bool IsInFront(CharacterView candidate)
    {
        Vector2 toTarget = (candidate.transform.position - m_Origin.position).normalized;

        float dot = Vector2.Dot(m_View.Direction, toTarget);

        float minDot = Mathf.Cos(ViewAngle * Mathf.Deg2Rad);

        return dot >= minDot;
    }

    private bool HasLineOfSight(CharacterView candidate)
    {
        Vector2 originPos = m_Origin.position;
        Vector2 targetPos = candidate.transform.position;
        Vector2 dir = targetPos - originPos;
        float distance = dir.magnitude;

        var hit = Physics2D.Raycast(originPos, dir.normalized, distance);

        return hit.collider == null;
    }
}