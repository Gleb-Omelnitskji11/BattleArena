using System.Collections.Generic;
using Game.Bullet;
using TowerDefence.Core;
using TowerDefence.Game;
using UnityEngine;

public class LevelProgressChecker : MonoBehaviour, IService
{
    [SerializeField]
    private LevelConfig m_LevelConfig;

    [SerializeField]
    private UnitSpawner m_UnitSpawner;

    private IEventBus m_EventBus;

    public readonly List<CharacterView> AllBots = new List<CharacterView>();
    public SeparatePlayerController Player { get; private set; }

    private void Start()
    {
        Init();
        NextLevel();
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance?.Unregister<LevelProgressChecker>();
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
                AllBots.Add(bot.CharacterView);
                bot.CharacterView.OnDie += CheckGameOver;
            }
        }
    }

    private void CheckGameOver()
    {
        foreach (var bot in AllBots)
        {
            if (bot.TeamId != Player.CharacterView.TeamId)
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
            AllBots[i].gameObject.SetActive(false);
        }

        Player?.CharacterView.gameObject.SetActive(false);
        BulletSpawner.Instance.Clear();
    }
}