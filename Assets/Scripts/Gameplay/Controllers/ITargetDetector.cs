using System.Collections.Generic;
using Gameplay.Views;

namespace Gameplay.Controllers
{
    public interface ITargetDetector
    {
        bool TryDetectTarget(out CharacterView target);
        void UpdateEnemies(List<CharacterView> allBots);
        bool IsEnemy(CharacterView candidate);
    }
}