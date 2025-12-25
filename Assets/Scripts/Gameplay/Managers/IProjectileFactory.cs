using Gameplay.Bullet;
using Gameplay.Models;
using UnityEngine;

namespace Gameplay.Managers
{
    public interface IProjectileFactory
    {
        Projectile Spawn(ProjectileType type, Transform spawnPoint, string teamId, int damage);

        void Clear();
    }
}