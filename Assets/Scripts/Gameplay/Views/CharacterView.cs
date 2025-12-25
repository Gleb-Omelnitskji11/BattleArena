using System;
using Gameplay.Bullet;
using Gameplay.Controllers;
using Gameplay.Managers;
using Gameplay.Models;
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

        private IMovementController m_MovementController;
        private IAttackExecutor m_AttackExecutor;
        private ICharacterController m_Controller;
        public TeamId TeamId => CharacterModel.TeamId;
        public Vector2 Direction => m_MovementController.MoveDirection;

        private bool m_Active;

        public void Init(CharacterModel model, IProjectileFactory projectileFactory)
        {
            CharacterModel = model;

            m_MovementController = new TankMovementController(transform, model.Movement);

            m_AttackExecutor = new SimpleAttackExecutor(m_ProjectileSpawnPoint, model.AttackComponent, model.Damage, projectileFactory);

            model.Health.OnDeath += HandleDeath;
        }

        public void SetTeam(TeamId teamId, bool isPlayer)
        {
            CharacterModel.ChangeTeam(teamId);
            gameObject.tag = teamId.ToString();

            m_TeamHighlight.color =
                isPlayer ? Color.green : CharacterModel.TeamId == TeamId.Red ? Color.red : Color.blue;
        }

        public void SetController(ICharacterController controller)
        {
            m_Controller = controller;
            m_Active = true;
        }

        private void Update()
        {
            if (!m_Active) return;

            m_Controller.Tick();
            m_MovementController.Move(Time.deltaTime);
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
            OnDie?.Invoke();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!m_Active) return;

            OnCollision?.Invoke(collision.gameObject);

            string enemyTag = GetEnemyTeamID().ToString();
            bool isCollisionWithEnemy = collision.gameObject.CompareTag(enemyTag);
            bool isProjectile = collision.gameObject.TryGetComponent<Projectile>(out Projectile projectile);

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
        public event Action OnDie;
    }
}