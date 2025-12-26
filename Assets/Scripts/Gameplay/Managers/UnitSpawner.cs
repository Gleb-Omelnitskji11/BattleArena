using Gameplay.Controllers;
using Gameplay.Models;
using TowerDefence.Core;
using TowerDefence.Data;
using TowerDefence.Systems;
using UnityEngine;

namespace Gameplay.Managers
{
    public class UnitSpawner : MonoBehaviour
    {
        private IProjectileFactory m_ProjectileFactory;
        private ICharactersFactory m_CharactersFactory;

        public void Start()
        {
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
            view.ActivateController(player);

            string chName = GetName(type, "PLAYER");
            view.name = chName;
            view.CharacterModel.Health.UpdateNam(chName);
            return player;
        }

        public SeparateBotController SpawnBot(RaceType type, int number, bool isEnemy)
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

            ITargetDetector detector = new DirectionalTargetDetector(view.transform, view);
            bot.SetData(view, detector);
            view.SetTeam(isEnemy ? TeamId.Red : TeamId.Blue, false);
            view.ActivateController(bot);

            string chName = GetName(type, number.ToString());
            view.name = chName;
            view.CharacterModel.Health.UpdateNam(chName);
            return bot;
        }

        private string GetName(RaceType type, string suffix)
        {
            return $"{type.ToString()}_{suffix}";
        }
    }
}