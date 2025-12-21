using System;
using Game.Bullet;
using Game.LevelSystem;
using Systems.InputService;
using TowerDefence.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform m_BulletSpawnPoint;

        private float m_MoveSpeed = 30;

        private IPlayerInputController m_PlayerInputController;

        private void Start()
        {
            SetMovingSystem();
        }

        private void SetMovingSystem()
        {
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
            Vector3 move = new Vector3(m_PlayerInputController.MoveInput.x, m_PlayerInputController.MoveInput.y, 0);
            transform.position += move * m_MoveSpeed * Time.deltaTime;
        }

        private void Shoot()
        {
            if (!m_PlayerInputController.ShootPressed)
            {
                return;
            }

            TransformModel transformModel = new TransformModel()
            {
                Position = m_BulletSpawnPoint.position,
                Rotation = m_BulletSpawnPoint.rotation
            };

            BulletSpawner.Instance.Shoot(transformModel, "PlayerBullet");
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
            gameObject.SetActive(false);
            Destroying?.Invoke();
        }

        public event Action Destroying;
    }
}