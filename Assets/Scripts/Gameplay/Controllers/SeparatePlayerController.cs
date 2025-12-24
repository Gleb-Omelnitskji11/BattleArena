using System;
using Game.Bullet;
using TowerDefence.Core;
using TowerDefence.Game;
using TowerDefence.Systems;
using UnityEngine;

public class SeparatePlayerController : ICharacterController, IDisposable
{
    private readonly CharacterView m_CharacterView;
    private CharacterModel m_Model;
    private IInputService m_PlayerInputController;

    private bool m_AttackPressed;
    private IEventBus m_EventBus;
    
    public CharacterView CharacterView => m_CharacterView;

    public SeparatePlayerController(CharacterView characterView)
    {
        m_CharacterView = characterView;

        SetSystems();
    }
    
    private void SetSystems()
    {
        m_EventBus = Services.Get<IEventBus>();

        if (Services.TryGet<IInputService>(out var inputService))
        {
            m_PlayerInputController = inputService;
        }
            
        m_PlayerInputController.OnHold += OnHold;
        m_PlayerInputController.OnTap += OnTap;
        m_PlayerInputController.OnTouchMoved += OnTouchMoved;
        m_PlayerInputController.OnCancel += OnTouchCancelled;
        
        m_CharacterView.OnCollision += OnCollision;
        m_CharacterView.OnDie += OnDie;
    }

    public void Dispose()
    {
        m_PlayerInputController.OnHold -= OnHold;
        m_PlayerInputController.OnTap -= OnTap;
        m_PlayerInputController.OnTouchMoved -= OnTouchMoved;
        m_PlayerInputController.OnCancel -= OnTouchCancelled;
        
        m_CharacterView.OnCollision -= OnCollision;
        m_CharacterView.OnDie -= OnDie;
    }

    public void Tick()
    {
    }
    
    private void OnHold(Vector2 screenPos)
    {
        m_CharacterView.UpdateDirection(screenPos);

        if(screenPos == Vector2.zero) m_CharacterView.StopMove();
    }

    private void OnTouchMoved(Vector2 screenPos)
    {
        m_CharacterView.UpdateDirection(screenPos);
        if(screenPos == Vector2.zero) m_CharacterView.StopMove();
    }

    private void OnTouchCancelled()
    {
        m_CharacterView.StopMove();
    }
        
    private void OnTap(Vector2 screenPos)
    {
        m_CharacterView.UpdateDirection(screenPos, false);
        m_CharacterView.Attack();
    }

    private void OnCollision(GameObject obj)
    {
        m_CharacterView.StopMove();
    }

    private void OnDie()
    {
        m_EventBus.Publish(new GameOverEvent());
    }
}