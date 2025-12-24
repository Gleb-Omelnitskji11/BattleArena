using Gameplay.Managers;
using Gameplay.Models;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class SimpleAttackExecutor : IAttackExecutor
    {
        private readonly Transform m_StartPoint;
        private readonly AttackComponent m_AttackComponent;
        private readonly int m_Damage;
        private readonly ProjectileType m_ProjectileType;

        public SimpleAttackExecutor(Transform startPoint, AttackComponent attackComponent, int damage)
        {
            m_StartPoint = startPoint;
            m_AttackComponent = attackComponent;
            m_Damage = damage;
        }

        public void TryAttack(string tag)
        {
            if (!m_AttackComponent.CanAttack(Time.time))
                return;

            BulletSpawner.Instance.SpawnProjectile(m_StartPoint, m_ProjectileType, tag, m_Damage);
            m_AttackComponent.RegisterAttack(Time.time);
        }
    }

    public interface IAttackExecutor
    {
        void TryAttack(string tag);
    }
}