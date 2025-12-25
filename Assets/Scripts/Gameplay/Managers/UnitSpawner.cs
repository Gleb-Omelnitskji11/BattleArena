using System;
using Gameplay.ConfigScripts;
using Gameplay.Controllers;
using Gameplay.Models;
using Gameplay.Views;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Managers
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField]
        private FieldView m_Field;

        [SerializeField]
        private LevelProgressChecker m_LevelProgressChecker;

        private IProjectileFactory m_ProjectileFactory;

        private CharactersConfig m_CharactersConfig;
        private bool m_Initialized;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (m_Initialized)
                return;

            m_Initialized = true;
            IConfigProvider configProvider = Services.Get<IConfigProvider>();

            if (configProvider.TryGet<CharactersConfig>("CharactersConfig", out CharactersConfig charactersConfig))
            {
                m_CharactersConfig = charactersConfig;
            }

            IObjectPooler pooler = Services.Get<IObjectPooler>();
            pooler.Init();
            m_ProjectileFactory = new ProjectileFactory(pooler, configProvider);
        }

        public void Clear()
        {
            m_ProjectileFactory.Clear();
        }

        public SeparatePlayerController SpawnPlayer(RaceType type)
        {
            var view = CreateNewCharacter(type);

            view.SetTeam(TeamId.Blue, true);
            view.transform.position = GetRandomPositionForNew(true);
            SeparatePlayerController player = new SeparatePlayerController(view);
            view.SetController(player);
            return player;
        }

        public SeparateBotController SpawnBot(RaceType type, bool isEnemy)
        {
            var view = CreateNewCharacter(type);
            view.SetTeam(isEnemy ? TeamId.Red : TeamId.Blue, false);
            ITargetDetector detector = new DirectionalTargetDetector(view.transform, view);
            view.transform.position = GetRandomPositionForNew(false);

            SeparateBotController bot = new SeparateBotController(view, detector);
            view.SetController(bot);
            return bot;
        }

        private CharacterView CreateNewCharacter(RaceType raceType)
        {
            switch (raceType)
            {
                case RaceType.Tank:
                    return CreateNewTank();
                default: throw new InvalidOperationException();
            }
        }

        private CharacterView CreateNewTank()
        {
            CharacterConfigModel characterConfigModel = m_CharactersConfig.GetCharacterModel(RaceType.Tank);
            HealthComponent healthComponent = new HealthComponent(characterConfigModel.MaxHealth);

            AttackComponent attackComponent =
                new AttackComponent(characterConfigModel.FireRate, characterConfigModel.Damage);

            MovementStats movementStats = new MovementStats(characterConfigModel.Speed);

            CharacterModel characterModel = new CharacterModel(TeamId.Neutral, RaceType.Tank, healthComponent,
                attackComponent, movementStats, characterConfigModel.Damage);

            CharacterView newUnit = Instantiate(characterConfigModel.CharacterPrefab);
            newUnit.Init(characterModel, m_ProjectileFactory);
            return newUnit;
        }

        private Vector2 GetRandomPositionForNew(bool isPlayer)
        {
            Vector2 spawnPos = m_Field.GetRandomPosition();
            int locker = 30;

            while (!IsSpawnPossible(spawnPos, !isPlayer))
            {
                if (--locker == 0)
                {
                    spawnPos = Vector2.zero;
                    break;
                }

                spawnPos = m_Field.GetRandomPosition();
            }

            return spawnPos;
        }

        private bool IsSpawnPossible(Vector2 placeToSpawn, bool playerCheck)
        {
            const int minAllowableDistance = 10;
            float distance;

            foreach (var enemy in m_LevelProgressChecker.AllBots)
            {
                if (enemy != null && !IsDistanceSufficient(enemy.CharacterView.transform.position, placeToSpawn))
                    return false;
            }

            if (playerCheck && !IsDistanceSufficient(m_LevelProgressChecker.Player.CharacterView.transform.position,
                    placeToSpawn))
            {
                return false;
            }

            return true;

            bool IsDistanceSufficient(Vector2 point1, Vector2 point2)
            {
                distance = Vector2.Distance(point1, point2);

                return distance >= minAllowableDistance;
            }
        }
    }
}