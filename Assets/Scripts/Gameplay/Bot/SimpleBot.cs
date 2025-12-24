using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Enemy
{
    public class SimpleBot : IBotBehavior
    {
        private const float MinTimeMoving = 1;
        private const float MaxTimeMoving = 5;
        private const float MoveSpeed = 20;

        private PatrollingData m_BehaviorData;
        private Transform m_Enemy;

        public void SetBehaviorData()
        {
            m_BehaviorData = new PatrollingData();
            ChooseDirection();
        }

        public void SetTransform(Transform obj)
        {
            m_Enemy = obj;
        }

        public void OnCollisionEnter(string objectTag)
        {
            ChooseDirection();
        }

        // public void OnCollisionStay(string objectTag)
        // {
        //     ChooseDirection();
        // }

        public void DoState()
        {
            Patrolling();
        }

        private void Patrolling()
        {
            m_BehaviorData.TimeMoving -= Time.deltaTime;

            if (m_BehaviorData.TimeMoving <= 0)
            {
                ChooseDirection();
            }

            Vector2 pos = m_Enemy.position;
            pos += m_BehaviorData.Direction * (MoveSpeed * Time.deltaTime);
            m_Enemy.position = pos;
        }

        private Vector2 GetRandomDirection()
        {
            int directionIndex = Random.Range(0, 4);

            Vector2 vector = directionIndex switch
            {
                0 => Vector2.up,
                1 => Vector2.down,
                2 => Vector2.left,
                3 => Vector2.right,
                _ => Vector2.zero
            };

            return vector;
        }

        private void ChooseDirection()
        {
            m_BehaviorData.TimeMoving = Random.Range(MinTimeMoving, MaxTimeMoving);

            m_BehaviorData.Direction = GetRandomDirection();
            float angle = Mathf.Atan2(m_BehaviorData.Direction.x, m_BehaviorData.Direction.y) * Mathf.Rad2Deg;
            m_Enemy.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
    }

    public class PatrollingData
    {
        public Vector2 Direction;
        public float TimeMoving;
    }
}