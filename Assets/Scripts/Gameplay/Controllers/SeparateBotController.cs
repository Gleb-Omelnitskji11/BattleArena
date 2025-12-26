using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gameplay.Models;
using Gameplay.Views;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class SeparateBotController : ICharacterController
    {
        private CharacterView m_CharacterView;
        private ITargetDetector m_Detector;

        private CharacterView m_CurrentTarget;
        public CharacterView CharacterView => m_CharacterView;

        private Action m_UpdateAction;
        private bool m_HasTarget;
        private CancellationTokenSource m_EscapeCts;
        private const int MaxTimeInOneDirection = 10000;
        private const int MaxEscapeAttempts = 3;
        
        
        private Vector2 m_CurrentDirection;

        public void SetData(CharacterView view, ITargetDetector detector)
        {
            m_CharacterView = view;
            m_Detector = detector;
            view.OnDie += OnDie;
            view.OnCollision += OnCollision;
            UpdatePatrol();
        }

        ~SeparateBotController()
        {
            m_CharacterView.StopMove();

            m_CharacterView.OnCollision -= OnCollision;
            m_CharacterView.OnDie -= OnDie;
        }

        public void ResetData()
        {
            m_UpdateAction = () => { };
            m_CurrentDirection = Vector2.zero;
            m_EscapeCts = null;
            m_HasTarget = false;
            m_CharacterView.Reset();
        }

        public void Tick()
        {
            m_UpdateAction();
        }

        private void UpdatePatrol()
        {
            m_CurrentDirection = GetRandomDirection();
            m_CharacterView.UpdateDirection(m_CurrentDirection);
            m_UpdateAction = Patrol;
        }

        private void Patrol()
        {
            if (!m_Detector.TryDetectTarget(out var target))
                return;

            m_CurrentTarget = target;
            m_HasTarget = true;
            m_UpdateAction = Pursuit;
        }

        private void Pursuit()
        {
            if (!m_HasTarget)
            {
                m_CurrentTarget = null;
                UpdatePatrol();
                return;
            }

            Vector2 dir = (m_CurrentTarget.transform.position - m_CharacterView.transform.position).normalized;

            dir = ClampToCardinal(dir);

            m_CharacterView.UpdateDirection(dir);

            if (IsTargetInFront(m_CharacterView.transform, m_CurrentDirection, m_CurrentTarget.transform))
            {
                m_CharacterView.Attack();
            }
        }

        public void UpdateTargets(List<CharacterView> allBots)
        {
            m_Detector.UpdateEnemies(allBots);

            if (m_HasTarget)
            {
                if (!m_Detector.IsEnemy(m_CurrentTarget))
                {
                    m_HasTarget = false;
                    UpdatePatrol();
                }
            }
        }

        private bool IsTargetInFront(Transform bot, Vector2 botForward, Transform target)
        {
            if (botForward == Vector2.zero)
                return false;

            Vector2 toTarget = ((Vector2)target.position - (Vector2)bot.position).normalized;

            float dot = Vector2.Dot(botForward, toTarget);
            float cosThreshold = Mathf.Cos(15f * Mathf.Deg2Rad); ///Todo make vigion from config

            return dot >= cosThreshold;
        }

        private void OnCollision(GameObject obj)
        {
            if (obj.CompareTag(m_CharacterView.CharacterModel.TeamId.ToString())
                || obj.CompareTag(TeamId.Neutral.ToString()))
            {
                StartEscape();
            }
        }

        private async void StartEscape()
        {
            CancelEscape();

            m_EscapeCts = new CancellationTokenSource();
            CancellationToken token = m_EscapeCts.Token;

            Vector2 originalDir = m_CurrentDirection;

            for (int attempt = 0; attempt < MaxEscapeAttempts; attempt++)
            {
                Vector2 newDir = GetEscapeDirection();
                m_CurrentDirection = newDir;
                m_CharacterView.UpdateDirection(newDir);

                try
                {
                    await Task.Delay((int)(MaxTimeInOneDirection * 1000), token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                if (m_CurrentDirection != originalDir)
                    return;
            }

            m_CurrentDirection = GetRandomDirection();
            m_CharacterView.UpdateDirection(m_CurrentDirection);
        }

        private Vector2 GetEscapeDirection()
        {
            if (m_CurrentDirection == Vector2.up || m_CurrentDirection == Vector2.down)
            {
                return UnityEngine.Random.value > 0.5f ? Vector2.left : Vector2.right;
            }

            if (m_CurrentDirection == Vector2.left || m_CurrentDirection == Vector2.right)
            {
                return UnityEngine.Random.value > 0.5f ? Vector2.up : Vector2.down;
            }

            return GetRandomDirection();
        }

        private void CancelEscape()
        {
            if (m_EscapeCts == null)
                return;

            m_EscapeCts.Cancel();
            m_EscapeCts.Dispose();
            m_EscapeCts = null;
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

        private void OnDie()
        {
            int restore = (int)(m_CharacterView.CharacterModel.Health.MaxHp * 0.6f);
            m_CharacterView.CharacterModel.Health.Restore(restore);

            TeamId enemyId = m_CharacterView.GetEnemyTeamID();
            m_CharacterView.SetTeam(enemyId, false);
        }
    }
}