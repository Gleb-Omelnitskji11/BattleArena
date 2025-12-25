using System;
using System.Collections.Generic;
using Gameplay.ConfigScripts;
using Gameplay.Controllers;
using Gameplay.Models;
using Gameplay.Views;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Game;
using UnityEngine;

namespace Gameplay.Managers
{
    public class LevelProgressChecker : MonoBehaviour, IService
    {
        [SerializeField]
        private UnitSpawner m_UnitSpawner;

        private IEventBus m_EventBus;
        private LevelConfig m_LevelConfig;

        public readonly List<SeparateBotController> AllBots = new List<SeparateBotController>();
        public SeparatePlayerController Player { get; private set; }

        private void Awake()
        {
            if (Services.TryGet<IConfigProvider>(out IConfigProvider configProvider))
            {
                if (configProvider.TryGet<LevelConfig>("LevelsConfig", out LevelConfig levelConfig))
                {
                    m_LevelConfig = levelConfig;
                }
            }
        }

        private void Start()
        {
            Init();
            NextLevel();
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance?.Unregister<LevelProgressChecker>();

            m_UnitSpawner.Clear();
            Player.CharacterView.OnDie -= GameOver;
        }

        public void Init()
        {
            m_EventBus = Services.Get<IEventBus>();
            ServiceLocator.Instance.Register(GetType(), this);
        }

        public void NextLevel()
        {
            LevelModel levelModel = m_LevelConfig.GetDefaultLevel();
            LoadLevel(levelModel);
        }

        private void LoadLevel(LevelModel levelModel)
        {
            ClearLevel();
            SpawnPlayer(levelModel.PlayerRace);
            SpawnBots(levelModel.Enemies, levelModel.Allies);
        }

        private void SpawnPlayer(RaceType playerRace)
        {
            Player = m_UnitSpawner.SpawnPlayer(playerRace);
            Player.CharacterView.OnDie += GameOver;
            Player.CharacterView.gameObject.SetActive(true);
        }

        private void SpawnBots(BotLevelModel[] enemyLevelModels, BotLevelModel[] alliesLevelModels)
        {
            foreach (var model in enemyLevelModels)
            {
                for (int i = 0; i < model.Amount; i++)
                {
                    SeparateBotController bot = m_UnitSpawner.SpawnBot(model.RaceType, true);
                    AllBots.Add(bot);
                    bot.CharacterView.OnDie += CheckGameOver;
                }
            }

            foreach (var model in alliesLevelModels)
            {
                for (int i = 0; i < model.Amount; i++)
                {
                    SeparateBotController bot = m_UnitSpawner.SpawnBot(model.RaceType, false);
                    AllBots.Add(bot);
                    bot.CharacterView.OnDie += CheckGameOver;
                }
            }
        }

        private void CheckGameOver()
        {
            foreach (var bot in AllBots)
            {
                bot.UpdateTargets(AllBots);

                if (bot.CharacterView.TeamId != Player.CharacterView.TeamId)
                    return;
            }

            GameOver();
        }

        private void GameOver()
        {
            m_EventBus.Publish(new GameOverEvent());
        }

        private void ClearLevel()
        {
            for (int i = 0; i < AllBots.Count; i++)
            {
                AllBots[i].CharacterView.gameObject.SetActive(false);
            }

            Player?.CharacterView.gameObject.SetActive(false);
        }
    }
}