using UnityEngine;

public interface IMovementController
{
    Vector2 Direction { get; }
    void SetDirection(Vector2 direction, bool canMove = true);
    void Move(float deltaTime);
    void Stop();
}