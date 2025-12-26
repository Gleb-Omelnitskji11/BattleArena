using System.Collections.Generic;
using Gameplay.ConfigScripts;
using Gameplay.Controllers;
using Gameplay.Models;
using Gameplay.Views;
using UnityEngine;

namespace Gameplay.Managers
{
    public class LevelLoader
    {
        private readonly UnitSpawner m_UnitSpawner;
        private FieldView m_Field;

        public readonly List<SeparateBotController> AllBots = new();
        public readonly List<CharacterView> AllViews = new();

        public SeparatePlayerController Player { get; private set; }

        public LevelLoader(UnitSpawner unitSpawner, FieldView field)
        {
            m_UnitSpawner = unitSpawner;
            m_Field = field;
        }

        public void LoadLevel(LevelModel levelModel)
        {
            ClearLevel();

            SpawnPlayer(levelModel.PlayerRace);
            SpawnBots(levelModel.Enemies, levelModel.Allies);

            ShuffleAll();
            UpdateTargets();
            StartLevel();
        }

        public void ClearLevel()
        {
            m_UnitSpawner.RealiseAll();
            AllBots.Clear();
            AllViews.Clear();
        }

        private void SpawnPlayer(RaceType playerRace)
        {
            if (Player == null)
            {
                Player = m_UnitSpawner.SpawnPlayer(playerRace);
            }
            else if (Player.CharacterView.CharacterModel.Race != playerRace)
            {
                Object.Destroy(Player.CharacterView.gameObject);
                Player = m_UnitSpawner.SpawnPlayer(playerRace);
            }
            else
            {
                Player.ResetData();
            }

            Shuffle(Player.CharacterView.transform, true);
            Player.CharacterView.gameObject.SetActive(true);

            AllViews.Add(Player.CharacterView);
        }

        private void SpawnBots(BotLevelModel[] enemies, BotLevelModel[] allies)
        {
            SpawnBotsInternal(enemies, isEnemy: true);
            SpawnBotsInternal(allies, isEnemy: false);
        }

        private void SpawnBotsInternal(BotLevelModel[] models, bool isEnemy)
        {
            foreach (var model in models)
            {
                for (int i = 0; i < model.Amount; i++)
                {
                    var bot = m_UnitSpawner.SpawnBot(model.RaceType, i, isEnemy);

                    AllBots.Add(bot);
                    AllViews.Add(bot.CharacterView);
                }
            }
        }

        public void UpdateTargets()
        {
            foreach (var bot in AllBots)
            {
                bot.UpdateTargets(AllViews);
            }
        }

        private void ShuffleAll()
        {
            Shuffle(Player.CharacterView.transform, true);

            foreach (var bot in AllBots)
            {
                Shuffle(Player.CharacterView.transform, false);
            }
        }

        private void StartLevel()
        {
            foreach (var view in AllViews)
            {
                view.Activate();
            }
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

            foreach (var enemy in AllBots)
            {
                if (enemy != null && !IsDistanceSufficient(enemy.CharacterView.transform.position, placeToSpawn))
                    return false;
            }

            if (playerCheck && !IsDistanceSufficient(Player.CharacterView.transform.position,
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