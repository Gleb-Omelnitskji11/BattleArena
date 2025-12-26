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
        private readonly IProjectileFactory m_ProjectileFactory;

        public SimpleAttackExecutor(Transform startPoint, AttackComponent attackComponent, int damage, IProjectileFactory projectileFactory)
        {
            m_StartPoint = startPoint;
            m_AttackComponent = attackComponent;
            m_Damage = damage;
            m_ProjectileFactory = projectileFactory;
        }

        public void TryAttack(string tag)
        {
            if (!m_AttackComponent.CanAttack(Time.time))
                return;

            m_ProjectileFactory.Spawn(m_ProjectileType, m_StartPoint, tag, m_Damage);
            m_AttackComponent.RegisterAttack(Time.time);
        }

        public void ResetData()
        {
            
        }
    }
}