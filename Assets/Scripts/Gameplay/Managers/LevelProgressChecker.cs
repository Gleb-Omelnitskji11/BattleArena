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
    public class LevelProgressChecker : MonoBehaviour, IService ///todo sepate levelLoader
    {
        [SerializeField]
        private UnitSpawner m_UnitSpawner;

        private IEventBus m_EventBus;
        private LevelConfig m_LevelConfig;

        public readonly List<SeparateBotController> AllBots = new List<SeparateBotController>();
        public readonly List<CharacterView> AllViews = new List<CharacterView>();
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
            if(Player == null) Player = m_UnitSpawner.SpawnPlayer(playerRace);
                
            else if (Player.CharacterView.CharacterModel.Race != playerRace)
            {
                Destroy(Player.CharacterView);
            }

            m_UnitSpawner.Shuffle(Player.CharacterView.transform, true);
            Player.CharacterView.OnDie += GameOver;
            Player.CharacterView.gameObject.SetActive(true);
        }

        private void SpawnBots(BotLevelModel[] enemyLevelModels, BotLevelModel[] alliesLevelModels)
        {
            AllViews.Clear();
            AllBots.Clear();

            foreach (var model in enemyLevelModels)
            {
                for (int i = 0; i < model.Amount; i++)
                {
                    SeparateBotController bot = m_UnitSpawner.SpawnBot(model.RaceType, true);
                    AllBots.Add(bot);
                    AllViews.Add(bot.CharacterView);
                    bot.CharacterView.OnDie += CheckGameOver;
                }
            }

            foreach (var model in alliesLevelModels)
            {
                for (int i = 0; i < model.Amount; i++)
                {
                    SeparateBotController bot = m_UnitSpawner.SpawnBot(model.RaceType, false);
                    AllBots.Add(bot);
                    AllViews.Add(bot.CharacterView);
                    bot.CharacterView.OnDie += CheckGameOver;
                }
            }

            AllViews.Add(Player.CharacterView);

            foreach (var bot in AllBots)
            {
                bot.UpdateTargets(AllViews);
            }
        }

        private void CheckGameOver()
        {
            foreach (var bot in AllBots)
            {
                bot.UpdateTargets(AllViews);

                if (bot.CharacterView.TeamId != Player.CharacterView.TeamId
                    && bot.CharacterView.gameObject.activeInHierarchy)
                    return;
            }

            GameOver();
        }

        private void GameOver()
        {
            Player.CharacterView.OnDie -= GameOver;
            m_EventBus.Publish(new GameOverEvent());
        }

        private void ClearLevel()
        {
            m_UnitSpawner.Clear();

            for (int i = 0; i < AllBots.Count; i++)
            {
                AllBots[i].CharacterView.gameObject.SetActive(false);
                Destroy(AllBots[i].CharacterView.gameObject);
            }

            Player?.CharacterView.gameObject.SetActive(false);
        }
    }
}