using Game.Bullet;
using Systems.InputService;
using TowerDefence.Core;
using TowerDefence.Game;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform m_BulletSpawnPoint;

        private float m_MoveSpeed = 30;
        private float m_FireDelay = 1f;

        private IPlayerInputController m_PlayerInputController;
        private IEventBus m_EventBus;
        private float m_CurrentDelay;

        private void Start()
        {
            SetSystems();
        }

        private void SetSystems()
        {
            m_EventBus = Services.Get<IEventBus>();

            if (Services.TryGet<IInputManager>(out var inputManager))
            {
                m_PlayerInputController = inputManager.GetCurrentPlayerInputController();
            }
        }

        private void Update()
        {
            Move();
            Shoot();
        }

        private void Move()
        {
            Vector2 direction = m_PlayerInputController.MoveInput;
            Vector3 move = new Vector3(direction.x, direction.y, 0);
            transform.position += move * m_MoveSpeed * Time.deltaTime;

            if (m_PlayerInputController.MoveInput.magnitude >= 1)
            {
                float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void Shoot()
        {
            if (m_CurrentDelay > 0)
            {
                m_CurrentDelay -= Time.deltaTime;
                return;
            }

            if (!m_PlayerInputController.ShootPressed)
            {
                return;
            }

            m_CurrentDelay = m_FireDelay;
            BulletSpawner.Instance.SpawnBullet(m_BulletSpawnPoint);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                OnColligionWithEnemy();
            }
        }

        private void OnColligionWithEnemy()
        {
            m_EventBus.Publish(new GameOverEvent());
            gameObject.SetActive(false);
        }
    }
}