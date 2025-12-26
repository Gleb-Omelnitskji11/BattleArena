using TowerDefence.Systems;

namespace Gameplay.Bullet
{
    public interface IPoolable
    {
        public void SetPoolableData(IObjectPooler pooler, string poolKey);
        void Deactivate();
    }
}