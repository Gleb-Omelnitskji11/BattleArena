using UnityEngine;

namespace Game.Bullet
{
    public class Bullet : Projectile
    {
        [SerializeField]
        private Rigidbody2D m_Rigidbody;

        public bool IsBusy { get; private set; }


        public void TurnOff()
        {
            m_Rigidbody.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
            IsBusy = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TurnOff();
        }
    }
}