using Gameplay.Controllers;
using Gameplay.Models;
using Gameplay.Views;
using UnityEngine;

namespace Gameplay.Managers
{
    public interface ICharactersFactory
    {
        CharacterView Spawn(RaceType type);

        void Clear();
        void RealiseAll();
    }
}