using Game.Player;
using TowerDefence.Core;

namespace Systems.InputService
{
    public interface IInputManager : IService
    {
        IPlayerInputController GetCurrentPlayerInputController();
    }
}