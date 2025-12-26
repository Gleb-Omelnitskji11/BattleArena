using System;
using Gameplay.Bullet;
using Gameplay.Controllers;
using Gameplay.Managers;
using Gameplay.Models;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Views
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_TeamHighlight;

        [SerializeField]
        private Transform m_ProjectileSpawnPoint;

        public CharacterModel CharacterModel { get; private set; }
        public ICharacterController CharacterController { get; private set; }

        private IMovementController m_MovementController;
        private IAttackExecutor m_AttackExecutor;

        private IObjectPooler m_Pooler;
        private string m_PoolKey;

        public TeamId TeamId => CharacterModel.TeamId;
        public Vector2 Direction => m_MovementController.MoveDirection;

        private bool m_Active;

        public void SetPoolableData(IObjectPooler pooler, string poolKey)
        {
            m_Pooler = pooler;
            m_PoolKey = poolKey;
        }

        public void Init(CharacterModel model, IProjectileFactory projectileFactory)
        {
            CharacterModel = model;

            m_MovementController = new TankMovementController(transform, model.Movement);

            m_AttackExecutor = new SimpleAttackExecutor(m_ProjectileSpawnPoint, model.AttackComponent, model.Damage,
                projectileFactory);

            model.Health.OnDeath += HandleDeath;
        }

        public void Reset()
        {
            CharacterModel.ResetData();
            m_MovementController.ResetData();
            m_AttackExecutor.ResetData();
        }

        public void SetTeam(TeamId teamId, bool isPlayer)
        {
            CharacterModel.ChangeTeam(teamId);
            gameObject.tag = teamId.ToString();

            m_TeamHighlight.color =
                isPlayer ? Color.green : CharacterModel.TeamId == TeamId.Red ? Color.red : Color.blue;
        }

        public void ActivateController(ICharacterController controller)
        {
            CharacterController = controller;
        }

        public void Activate()
        {
            m_Active = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!m_Active) return;

            CharacterController.Tick();
            m_MovementController.Move(Time.deltaTime);
        }

        public void Deactivate()
        {
            StopMove();
            m_Pooler.Release(m_PoolKey, this);
            m_Active = false;
            gameObject.SetActive(false);
        }

        public void StopMove()
        {
            m_MovementController.Stop();
        }

        public void UpdateDirection(Vector2 screenPos, bool canMove = true)
        {
            m_MovementController.SetDirection(screenPos, canMove);
        }

        public void Attack()
        {
            m_AttackExecutor.TryAttack(CharacterModel.TeamId.ToString());
        }

        public void TakeDamage(int damage)
        {
            CharacterModel.Health.TakeDamage(damage);
        }

        private void HandleDeath()
        {
            OnDie?.Invoke(this);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OperateCollision(collision.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OperateCollision(collision.gameObject);
        }

        private void OperateCollision(GameObject other)
        {
            if (!m_Active) return;

            OnCollision?.Invoke(other);

            string enemyTag = GetEnemyTeamID().ToString();
            bool isCollisionWithEnemy = other.CompareTag(enemyTag);
            bool isProjectile = other.TryGetComponent<Projectile>(out Projectile projectile);

            if (isCollisionWithEnemy && isProjectile)
            {
                TakeDamage(projectile.Damage);
            }
        }

        public TeamId GetEnemyTeamID()
        {
            if (CharacterModel.TeamId == TeamId.Red)
                return TeamId.Blue;

            return TeamId.Red;
        }

        public event Action<GameObject> OnCollision;
        public event Action<CharacterView> OnDie;
    }
}