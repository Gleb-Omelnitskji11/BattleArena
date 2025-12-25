using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Bullet
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private Rigidbody2D m_Rigidbody;

        private IObjectPooler m_Pooler;
        private string m_PoolKey;

        public int Damage { get; private set; }

        public void Init(IObjectPooler pooler, string poolKey)
        {
            m_Pooler = pooler;
            m_PoolKey = poolKey;
        }

        public void Activate(int damage, float bulletSpeed)
        {
            gameObject.SetActive(true);
            Damage = damage;
            m_Rigidbody.linearVelocity = transform.up * bulletSpeed;
        }

        public void Deactivate()
        {
            m_Rigidbody.linearVelocity = Vector2.zero;
            m_Pooler?.Release(m_PoolKey, this);
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile)) //not projectile
            {
                Deactivate();
            }
        }
    }
}