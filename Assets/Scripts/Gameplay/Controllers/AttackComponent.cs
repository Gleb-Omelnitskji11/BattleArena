using UnityEngine;

namespace Gameplay.Controllers
{
    public class AttackComponent
    {
        private float FireRate { get; }
        public int Damage { get; }

        private float m_LastAttackTime;

        public AttackComponent(float fireRate, int damage)
        {
            FireRate = fireRate;
            Damage = damage;
        }

        public bool CanAttack(float time)
        {
            return time - m_LastAttackTime >= FireRate;
        }

        public void RegisterAttack(float time)
            => m_LastAttackTime = time;
    }
}