using System.Collections.Generic;
using Gameplay.Controllers;
using Gameplay.Managers;
using Gameplay.Models;
using Gameplay.Views;
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
    

    public DirectionalTargetDetector(Transform origin, CharacterView characterView)
    {
        m_Origin = origin;
        m_View = characterView;
    }

    public void UpdateEnemies(List<SeparateBotController> allBots)
    {
        foreach (var character in allBots)
        {
            if (!IsEnemy(character.CharacterView))
                continue;

            AllEnemies.Add(character.CharacterView);
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
                //if character2 further then character1
                if ((target.transform.position - m_Origin.position).magnitude
                    <= (character.transform.position - m_Origin.position).magnitude)
                    continue;
            }

            Debug.Log("FindTarget");
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