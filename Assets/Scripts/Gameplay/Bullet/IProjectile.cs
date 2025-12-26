using TowerDefence.Systems;

namespace Gameplay.Bullet
{
    public interface IProjectile : IPoolable
    {
        void Activate(int damage, float bulletSpeed);
    }
}