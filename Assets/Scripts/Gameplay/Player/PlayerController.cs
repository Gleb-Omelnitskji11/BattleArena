using System;
using Game.Bullet;
using Game.LevelSystem;
using TowerDefence.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform m_BulletSpawnPoint;
        
        private float m_MoveSpeed = 30;

        private IPlayerInputController m_PlayerMovement;

        private void Start()
        {
            SetMovingSystem();
        }

        private void SetMovingSystem()
        {
            //Services.TryGet<IPlayerInputController>(out var playerInputController);

            // m_PlayerMovement = m_PlayerMovementConfig.GetMovementSystem(MovementSystemType.Tetris);
            // m_PlayerMovement.Init(transform);
            // m_PlayerMovement.Shooting += OnShoot;
        }

        private void Update()
        {
            // Vector3 move = new Vector3(input.MoveInput.x, input.MoveInput.y, 0);
            // transform.position += move * m_MoveSpeed * Time.deltaTime;
            //
            // if (input.ShootPressed)
            // {
            //     Shoot();
            // }
        }

        private void Shoot()
        {
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
                DeleteTank();
            }
        }

        private void DeleteTank()
        {
            gameObject.SetActive(false);
            Destroying?.Invoke();
        }

        public event Action Destroying;
    }
}