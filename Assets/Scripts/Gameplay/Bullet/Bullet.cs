using UnityEngine;

namespace Game.Bullet
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_Rigidbody;

        public bool IsBusy { get; private set; }

        public void StartMove(float bulletSpeed)
        {
            IsBusy = true;
            m_Rigidbody.linearVelocity = transform.up * bulletSpeed;
        }

        public void TurnOff()
        {
            m_Rigidbody.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
            IsBusy = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag.Equals(gameObject.tag)) return;
            Debug.Log($"Bullet collision with obj {collision.gameObject.name} and tag {collision.gameObject.tag}");
            TurnOff();
        }
    }
}