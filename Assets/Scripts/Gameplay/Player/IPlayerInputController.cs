using System;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayerInputController : IDisposable
    {
        Vector2 MoveInput { get;}
        bool ShootPressed { get;}

        void Subscribe();
    }
}