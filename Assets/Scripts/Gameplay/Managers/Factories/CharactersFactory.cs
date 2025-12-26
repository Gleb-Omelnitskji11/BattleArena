using System;
using System.Collections.Generic;
using Gameplay.ConfigScripts;
using Gameplay.Controllers;
using Gameplay.Models;
using Gameplay.Views;
using TowerDefence.Data;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Managers
{
    public class CharactersFactory : ICharactersFactory
    {
        private readonly IObjectPooler m_Pooler;
        private readonly CharactersConfig m_CharactersConfig;
        private IProjectileFactory m_ProjectileFactory;

        private readonly List<CharacterView> m_Characters = new List<CharacterView>();

        public CharactersFactory(IObjectPooler pooler, IConfigProvider configProvider,
            IProjectileFactory projectileFactory)
        {
            m_Pooler = pooler;
            configProvider.TryGet("ProjectilesConfig", out m_CharactersConfig);
            m_ProjectileFactory = projectileFactory;
            InitPools();
        }

        private void InitPools()
        {
            foreach (RaceType type in Enum.GetValues(typeof(RaceType)))
            {
                CharacterConfigModel model = m_CharactersConfig.GetCharacterModel(type);
                string key = GetKey(type);

                Func<CharacterView> creator = GetCreatorFunc(type);

                m_Pooler.CreatePool(key, factory: creator, onGet: OnGetFromPool, onRelease: OnRealiseToPool,
                    prewarmCount: 0);
            }
        }

        private Func<CharacterView> GetCreatorFunc(RaceType raceType)
        {
            switch (raceType)
            {
                case RaceType.Tank:
                    return CreateNewTank;
            }

            throw new InvalidOperationException();
        }

        private void OnGetFromPool(CharacterView character)
        {
            character.gameObject.SetActive(true);
            character.Reset();
            m_Characters.Add(character);
        }

        private void OnRealiseToPool(CharacterView character)
        {
            character.gameObject.SetActive(false);
            m_Characters.Remove(character);
        }

        public void RealiseAll()
        {
            foreach (var character in m_Characters)
            {
                character.Deactivate();
            }
        }

        public CharacterView Spawn(RaceType type)
        {
            string key = GetKey(type);
            var character = m_Pooler.Get<CharacterView>(key);

            character.SetPoolableData(m_Pooler, key);

            return character;
        }

        public void Clear()
        {
            foreach (RaceType type in Enum.GetValues(typeof(RaceType)))
            {
                m_Pooler.Clear(GetKey(type));
            }
        }

        private CharacterView CreateNewTank()
        {
            CharacterConfigModel characterConfigModel = m_CharactersConfig.GetCharacterModel(RaceType.Tank);
            HealthComponent healthComponent = new(characterConfigModel.MaxHealth);

            AttackComponent attackComponent = new(characterConfigModel.FireRate, characterConfigModel.Damage);

            MovementStats movementStats = new(characterConfigModel.Speed);

            CharacterModel characterModel = new(TeamId.Neutral, RaceType.Tank, healthComponent, attackComponent,
                movementStats, characterConfigModel.Damage);

            CharacterView newUnit = GameObject.Instantiate(characterConfigModel.CharacterPrefab);
            newUnit.Init(characterModel, m_ProjectileFactory);
            return newUnit;
        }

        private static string GetKey(RaceType type) => $"Race_{type}";
    }
}