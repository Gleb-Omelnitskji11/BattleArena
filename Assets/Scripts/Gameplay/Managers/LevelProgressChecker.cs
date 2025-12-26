using Gameplay.ConfigScripts;
using Gameplay.Controllers;
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
        private FieldView m_Field;

        [SerializeField]
        private UnitSpawner m_UnitSpawner;

        private IEventBus m_EventBus;
        private LevelConfig m_LevelConfig;
        private LevelLoader m_LevelLoader;

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

            if (m_LevelLoader?.Player != null)
            {
                m_LevelLoader.Player.CharacterView.OnDie -= OnPlayerDie;
            }

            //m_LevelLoader?.ClearLevel();
        }

        public void Init()
        {
            m_EventBus = Services.Get<IEventBus>();
            m_LevelLoader = new LevelLoader(m_UnitSpawner, m_Field);

            ServiceLocator.Instance.Register(GetType(), this);
        }

        public void NextLevel()
        {
            var levelModel = m_LevelConfig.GetDefaultLevel();
            m_LevelLoader.LoadLevel(levelModel);

            SubscribeToDeaths();
        }

        private void SubscribeToDeaths()
        {
            foreach (var bot in m_LevelLoader.AllBots)
            {
                bot.CharacterView.OnDie += OnBotDie;
            }

            m_LevelLoader.Player.CharacterView.OnDie += OnPlayerDie;
        }

        private void OnBotDie(CharacterView view)
        {
            (view.CharacterController as SeparateBotController).ChangeTeam();

            if (CheckGameOver())
                GameOver(true);
            else
                m_LevelLoader.UpdateTargets();
        }

        private bool CheckGameOver()
        {
            foreach (var bot in m_LevelLoader.AllBots)
            {
                if (bot.CharacterView.TeamId != m_LevelLoader.Player.CharacterView.TeamId
                    && bot.CharacterView.gameObject.activeInHierarchy)
                {
                    bot.UpdateTargets(m_LevelLoader.AllViews);
                    return false;
                }
            }

            return true;
        }

        private void OnPlayerDie(CharacterView view)
        {
            GameOver(false);
        }

        private void GameOver(bool isWin)
        {
            if (isWin)
            {
                Debug.Log("You win");
            }
            else
            {
                Debug.Log("You are Dead");
            }

            foreach (var bot in m_LevelLoader.AllBots)
            {
                bot.CharacterView.OnDie -= OnBotDie;
            }
            m_LevelLoader.Player.CharacterView.OnDie -= OnPlayerDie;
            m_EventBus.Publish(new GameOverEvent());
        }
    }
}