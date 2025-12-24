using System.Collections.Generic;
using Gameplay.Bullet;
using Gameplay.ConfigScripts;
using Gameplay.Models;
using TowerDefence.Core;
using TowerDefence.Data;
using UnityEngine;

namespace Gameplay.Managers
{
    public class BulletSpawner : MonoBehaviour
    {
        private readonly Dictionary<ProjectileType, List<Projectile>> m_PooledProjectiles =
            new Dictionary<ProjectileType, List<Projectile>>();

        private ProjectileConfig m_ProjectileConfig;

        public static BulletSpawner Instance;

        private void Awake()
        {
            if (Services.TryGet<IConfigProvider>(out IConfigProvider configProvider))
            {
                if (configProvider.TryGet<ProjectileConfig>("ProjectilesConfig", out ProjectileConfig projectileConfig))
                {
                    m_ProjectileConfig = projectileConfig;
                }
            }
            
            
            Instance = this;
        }

        public void Clear()
        {
            foreach (var projectileList in m_PooledProjectiles)
            {
                foreach (var projectile in projectileList.Value)
                {
                    projectile.gameObject.SetActive(false);
                }
            }
        }

        public void SpawnProjectile(Transform startPoint, ProjectileType projectileType, string teamId, int damage)
        {
            var projectile = GetProjectile(projectileType);
            var projectileTransform = projectile.transform;
            projectileTransform.position = startPoint.position;
            projectileTransform.rotation = startPoint.rotation;
            projectile.tag = teamId;

            var model = m_ProjectileConfig.GetCharacterModel(projectileType);
            
            projectile.gameObject.SetActive(true);
            projectile.Activate(damage, model.Speed);
        }

        private Projectile GetProjectile(ProjectileType projectileType)
        {
            if (!m_PooledProjectiles.TryGetValue(projectileType, out List<Projectile> pooledProjectile))
                return CreateNew(projectileType);

            foreach (var projectile in pooledProjectile)
            {
                if (!projectile.gameObject.activeInHierarchy)
                {
                    return projectile;
                }
            }

            return CreateNew(projectileType);
        }

        private Projectile CreateNew(ProjectileType projectileType)
        {
            var model = m_ProjectileConfig.GetCharacterModel(projectileType);
            Projectile projectile = Instantiate(model.ProjectilePrefab);
            List<Projectile> projectiles = new List<Projectile>();
            projectiles.Add(projectile);

            if (m_PooledProjectiles.TryGetValue(projectileType, out List<Projectile> pooledProjectile))
                pooledProjectile.Add(projectile);

            else m_PooledProjectiles.Add(projectileType, projectiles);

            return projectile;
        }
    }
}