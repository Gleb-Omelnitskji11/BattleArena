using Gameplay.Views;

namespace Gameplay.Controllers
{
    public interface ITargetDetector
    {
        bool TryDetectTarget(out CharacterView target);
        void UpdateEnemies();
    }
}