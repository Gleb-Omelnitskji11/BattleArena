using System;
using System.Collections.Generic;
using Gameplay.Bullet;
using Gameplay.ConfigScripts;
using Gameplay.Models;
using TowerDefence.Data;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Managers
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly IObjectPooler m_Pooler;
        private readonly ProjectileConfig m_ProjectileConfig;
        
        private readonly List<IProjectile> m_Projectiles = new List<IProjectile>();

        public ProjectileFactory(IObjectPooler pooler, IConfigProvider configProvider)
        {
            m_Pooler = pooler;
            configProvider.TryGet("ProjectilesConfig", out m_ProjectileConfig);

            InitPools();
        }

        private void InitPools()
        {
            foreach (ProjectileType type in Enum.GetValues(typeof(ProjectileType)))
            {
                ProjectileConfigModel model = m_ProjectileConfig.GetCharacterModel(type);
                string key = GetKey(type);

                m_Pooler.CreatePool(key, factory: () => GameObject.Instantiate(model.ProjectilePrefab),
                    onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: model.PrewarmCount);
            }
        }

        private void OnGetFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
            m_Projectiles.Add(projectile);
        }
        
        private void OnRealiseToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            m_Projectiles.Remove(projectile);
        }

        public void RealiseAll()
        {
            foreach (var projectile in m_Projectiles)
            {
                projectile.Deactivate();
            }
        }

        public Projectile Spawn(ProjectileType type, Transform spawnPoint, string teamId, int damage)
        {
            string key = GetKey(type);
            var projectile = m_Pooler.Get<Projectile>(key);

            projectile.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            projectile.tag = teamId;

            var model = m_ProjectileConfig.GetCharacterModel(type);
            projectile.SetPoolableData(m_Pooler, key);
            projectile.Activate(damage, model.Speed);

            return projectile;
        }

        public void Clear()
        {
            foreach (ProjectileType type in Enum.GetValues(typeof(ProjectileType)))
            {
                m_Pooler.Clear(GetKey(type));
            }
        }

        private static string GetKey(ProjectileType type) => $"Projectile_{type}";
    }
}