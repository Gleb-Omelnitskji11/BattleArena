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
        private ICharactersFactory m_CharactersFactory;
        private bool m_Initialized;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (m_Initialized)
                return;

            m_Initialized = true;
            IConfigProvider configProvider = Services.Get<IConfigProvider>();

            IObjectPooler pooler = Services.Get<IObjectPooler>();
            pooler.Init();
            m_ProjectileFactory = new ProjectileFactory(pooler, configProvider);
            m_CharactersFactory = new CharactersFactory(pooler, configProvider, m_ProjectileFactory);
        }

        public void RealiseAll()
        {
            m_ProjectileFactory.RealiseAll();
            m_CharactersFactory.RealiseAll();
        }

        public SeparatePlayerController SpawnPlayer(RaceType type)
        {
            SeparatePlayerController player;
            var view = m_CharactersFactory.Spawn(type);
            ICharacterController character = view.CharacterController;

            if (character == null || !(character is SeparatePlayerController))
            {
                player = new SeparatePlayerController();
            }
            else
            {
                player = (SeparatePlayerController)view.CharacterController;
            }

            player.SetData(view);
            view.SetTeam(TeamId.Blue, true);
            view.Activate(player);
            Shuffle(view.transform, true);
            return player;
        }

        public SeparateBotController SpawnBot(RaceType type, bool isEnemy)
        {
            SeparateBotController bot;
            var view = m_CharactersFactory.Spawn(type);
            
            ICharacterController character = view.CharacterController;

            if (character == null || !(character is SeparateBotController))
            {
                bot = new SeparateBotController();
            }
            else
            {
                bot = (SeparateBotController)view.CharacterController;
            }
            
            Shuffle(view.transform, false);
            ITargetDetector detector = new DirectionalTargetDetector(view.transform, view);
            bot.SetData(view, detector);
            view.SetTeam(isEnemy ? TeamId.Red : TeamId.Blue, false);
            view.Activate(bot);
            return bot;
        }

        public void Shuffle(Transform objTransform, bool isPlayer)
        {
            objTransform.position = GetRandomPositionForNew(isPlayer);
        }

        private Vector2 GetRandomPositionForNew(bool isPlayer)
        {
            Vector2 spawnPos = m_Field.GetRandomPosition();
            int locker = 30;

            while (!IsSpawnInThisPossible(spawnPos, !isPlayer))
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

        private bool IsSpawnInThisPossible(Vector2 placeToSpawn, bool playerCheck)
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