namespace Gameplay.Controllers
{
    public interface IAttackExecutor : IComponent
    {
        void TryAttack(string tag);
    }
}