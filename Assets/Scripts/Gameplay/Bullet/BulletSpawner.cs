using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Bullet
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private ProjectileConfig m_ProjectileConfig;

        private readonly Dictionary<ProjectileType, List<Projectile>> m_PooledProjectiles =
            new Dictionary<ProjectileType, List<Projectile>>();

        public static BulletSpawner Instance;

        private void Awake()
        {
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

        public void SpawnProjectile(Transform startPoint, ProjectileType projectileType, string teamId)
        {
            var projectile = GetProjectile(projectileType);
            var projectileTransform = projectile.transform;
            projectileTransform.position = startPoint.position;
            projectileTransform.rotation = startPoint.rotation;
            projectile.tag = teamId;

            var model = m_ProjectileConfig.GetCharacterModel(projectileType);
            projectile.StartMove(model.Speed);
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
            projectile.gameObject.SetActive(true);
            List<Projectile> projectiles = new List<Projectile>();
            projectiles.Add(projectile);

            if (m_PooledProjectiles.TryGetValue(projectileType, out List<Projectile> pooledProjectile))
                pooledProjectile.Add(projectile);

            else m_PooledProjectiles.Add(projectileType, projectiles);

            return projectile;
        }
    }
}