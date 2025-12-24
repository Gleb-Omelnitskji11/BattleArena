using System;
using System.Linq;
using Gameplay.Models;
using Gameplay.Views;
using UnityEngine;

namespace Gameplay.ConfigScripts
{
    [CreateAssetMenu(fileName = "CharactersConfig", menuName = "Scriptable Objects/CharactersConfig", order = 0)]
    public class CharactersConfig : ScriptableObject
    {
        [SerializeField]
        private CharacterConfigModel[] m_CharacterModels;


        public CharacterConfigModel GetCharacterModel(RaceType characterType)
        {
            return m_CharacterModels.FirstOrDefault(x => x.CharacterType == characterType);
        }
    }

    [Serializable]
    public struct CharacterConfigModel
    {
        public RaceType CharacterType;
        public int MaxHealth;
        public int Speed;
        public int Damage;
        public float FireRate;
        public CharacterView CharacterPrefab;
    }
}