using TowerDefence.Systems;

namespace Gameplay.Bullet
{
    public interface IProjectile
    {
        public void Init(IObjectPooler pooler, string poolKey);

        void Activate(int damage, float bulletSpeed);
    }
}