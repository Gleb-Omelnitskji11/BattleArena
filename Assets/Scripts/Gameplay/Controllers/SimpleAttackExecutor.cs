using Gameplay.Managers;
using Gameplay.Models;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class SimpleAttackExecutor : IAttackExecutor
    {
        private readonly Transform m_StartPoint;
        private readonly AttackComponent m_AttackComponent;
        private readonly string m_AllyTag;
        private readonly ProjectileType m_ProjectileType;

        public SimpleAttackExecutor(Transform startPoint, AttackComponent attackComponent, string allyTag)
        {
            m_StartPoint = startPoint;
            m_AttackComponent = attackComponent;
            m_AllyTag = allyTag;
        }

        public void TryAttack()
        {
            if (!m_AttackComponent.CanAttack(Time.time))
                return;

            BulletSpawner.Instance.SpawnProjectile(m_StartPoint, m_ProjectileType, m_AllyTag);
            m_AttackComponent.RegisterAttack(Time.time);
        }
    }

    public interface IAttackExecutor
    {
        void TryAttack();
    }
}