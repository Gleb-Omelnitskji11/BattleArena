using UnityEngine;

namespace Gameplay.Bullet
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_Rigidbody;

        public int Damage { get; private set; }

        public void Activate(int damage, float bulletSpeed)
        {
            Damage = damage;
            m_Rigidbody.linearVelocity = transform.up * bulletSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile)) //not projectile
            {
                m_Rigidbody.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }
}