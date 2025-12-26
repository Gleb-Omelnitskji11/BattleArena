using System;
using UnityEngine;

namespace Gameplay.Controllers
{
    public class HealthComponent : IComponent
    {
        public int MaxHp { get; }
        public int CurrentHp { get; private set; }
        private string m_CurName;

        public event Action OnDeath;

        public HealthComponent(int maxHp)
        {
            MaxHp = maxHp;
        }

        public void UpdateNam(string curName)
        {
            m_CurName = curName;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Mathf.Max(0, CurrentHp - damage);
            Debug.Log($"{m_CurName} took {damage} damage and have {CurrentHp} hp");

            if (CurrentHp == 0)
                OnDeath?.Invoke();
        }

        public void Restore(int value)
        {
            CurrentHp = Mathf.Min(MaxHp, value);
        }

        public void ResetData()
        {
            CurrentHp = MaxHp;
        }
    }
}