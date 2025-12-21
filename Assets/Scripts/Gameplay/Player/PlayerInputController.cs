using UnityEngine;

namespace Game.Player
{
    public interface IPlayerInputController
    {
        Vector2 MoveInput { get;}
        bool ShootPressed { get;}
    }
}