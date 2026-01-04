using System;
using UnityEngine;

namespace Game.Player
{
    public interface IPlayerInputController : IDisposable
    {
        Vector2 MoveInput { get;}
        void Init();
        
        void Enable();
        void Disable();
        bool IsEnabled { get; }

        
        event Action OnFire;
        event Action<Vector2, bool> OnNewDirection;
        event Action OnMoveCancel;
    }
}