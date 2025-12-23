using System.Collections.Generic;
using Game.Bullet;
using Game.Enemy;
using Game.LevelSystem;
using Game.Player;
using Game.Tanks;
using TowerDefence.Core;
using TowerDefence.Game;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelProgressChecker : MonoBehaviour, IService
{
    [SerializeField]
    private FieldView m_FieldView;

    [SerializeField]
    private LevelConfig m_LevelConfig;

    [SerializeField]
    private PlayerController m_PlayerPrefab;

    private PlayerController m_Player;
    private readonly List<BotController> m_Enemies = new List<BotController>();
    private IEventBus m_EventBus;

    private void Start()
    {
        Init();
        NextLevel();
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance?.Unregister<LevelProgressChecker>();

        m_FieldView.DestroyedAllEnemies -= NextLevel;
    }

    public void Init()
    {
        m_EventBus = Services.Get<IEventBus>();
        ServiceLocator.Instance.Register(GetType(), this);
        
        m_Player = Instantiate(m_PlayerPrefab);
        m_FieldView.Init(m_Player);
        m_FieldView.DestroyedAllEnemies += GameOver;
    }

    public void NextLevel()
    {
        LevelModel levelModel = m_LevelConfig.GetDefaultLevel();
        LoadLevel(levelModel);
    }

    private void LoadLevel(LevelModel levelModel)
    {
        ClearLevel();
        SetPlayer();
        SpawnEnemies(levelModel.Enemies);
    }

    private void GameOver()
    {
        m_EventBus.Publish(new GameOverEvent());
    }

    private void SetPlayer()
    {
        m_Player.transform.position = m_FieldView.GetRandomPositionForPlayer();
        m_Player.transform.rotation = Quaternion.identity;
        m_Player.gameObject.SetActive(true);
    }

    private void SpawnEnemies(EnemyLevelModel[] enemyLevelModels)
    {
        Vector2 tempVector = default;
        Quaternion tempQuaternion = default;

        foreach (var enemyType in enemyLevelModels)
        {
            for (int i = 0; i < enemyType.Amount; i++)
            {
                BotController bot = m_FieldView.SpawnEnemy(tempVector, tempQuaternion, enemyType.BotType);
                m_Enemies.Add(bot);
                bot.Destroying += OnEnemyDestroy;
            }
        }
    }

    private void OnEnemyDestroy(BotController botController)
    {
        m_Enemies.Remove(botController);
    }

    private void ClearLevel()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i] != null)
            {
                Destroy(m_Enemies[i].gameObject);
            }
        }

        m_FieldView.ClearData();
        m_Enemies.Clear();
        BulletSpawner.Instance.Clear();
    }
}