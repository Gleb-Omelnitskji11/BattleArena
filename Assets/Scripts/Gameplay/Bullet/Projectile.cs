using UnityEngine;

namespace Game.Bullet
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_Rigidbody;

        public int Damage { get; private set; }

        public void StartMove(float bulletSpeed)
        {
            m_Rigidbody.linearVelocity = transform.up * bulletSpeed;
        }
    }
}