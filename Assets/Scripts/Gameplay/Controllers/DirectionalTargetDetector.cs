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
    private const float ViewAngle = 15f;//Todo get value from config

    private readonly List<CharacterView> AllEnemies = new List<CharacterView>();
    private CharacterModel CharacterModel => m_View.CharacterModel;

    public DirectionalTargetDetector(Transform origin, CharacterView characterView)
    {
        m_Origin = origin;
        m_View = characterView;
    }

    public void UpdateEnemies(List<CharacterView> allBots)
    {
        AllEnemies.Clear();
        foreach (var character in allBots)
        {
            if (!IsEnemy(character))
                continue;

            AllEnemies.Add(character);
        }
    }

    public bool TryDetectTarget(out CharacterView target)
    {
        target = null;
        bool detected = false;

        if (m_View.Direction == Vector2.zero)
            return false;

        foreach (var character in AllEnemies)
        {
            if (!IsInFront(character))
                continue;

            // if (!HasLineOfSight(character))
            //     continue;

            if (!detected)
            {
                target = character;
                detected = true;
            }
            else if ((target.transform.position - m_Origin.position).magnitude
                     <= (character.transform.position - m_Origin.position).magnitude)
                continue;

            target = character;
        }

        return detected;
    }

    public bool IsEnemy(CharacterView candidate)
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