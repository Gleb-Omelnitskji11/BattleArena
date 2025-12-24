using System;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class HealthComponent
    {
        public int MaxHp { get; }
        public int CurrentHp { get; private set; }

        public event Action OnDeath;

        public HealthComponent(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(0, CurrentHp - damage);

            if (CurrentHp == 0)
                OnDeath?.Invoke();
        }

        public void Restore(int value)
        {
            CurrentHp = Mathf.Min(MaxHp, value);
        }
    }
}