using System;
using System.Collections.Generic;
using System.Linq;
using Game.Enemy;
using Game.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Tanks
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_Borders;

        [SerializeField]
        private EnemyConfig m_EnemyConfig;

        private readonly List<BotController> m_Enemies = new List<BotController>();
        private PlayerController m_Player;
        private Bounds m_FieldForSpawn;

        public void Init(PlayerController player)
        {
            m_Player = player;
            SetSpawnField();
        }

        public void ClearData()
        {
            m_Enemies.Clear();
        }

        public Vector2 GetRandomPositionForPlayer()
        {
            Vector2 spawnPos = GetRandomPosition();
            int locker = 30;

            while (!IsSpawnPossible(spawnPos, false))
            {
                if (--locker == 0)
                {
                    spawnPos = Vector2.zero;
                    Debug.LogWarning($"Set default player position");
                    break;
                }

                spawnPos = GetRandomPosition();
            }

            Debug.Log($"player start position {spawnPos.x}, {spawnPos.y}");
            return spawnPos;
        }

        public BotController SpawnEnemy(Vector2 spawnPosition, Quaternion spawnRotation, BotType botType)
        {
            EnemyModel enemyPrefab = m_EnemyConfig.GetEnemyModel(botType);
            IBotBehavior botBehavior = m_EnemyConfig.GetEnemyBehavior(botType);

            if (spawnPosition == default)
            {
                spawnPosition = GetRandomPositionForEnemy();
                spawnRotation = Quaternion.identity;
            }

            BotController newBot = Instantiate(enemyPrefab.TankPrefab, spawnPosition, spawnRotation);

            newBot.Destroying += ClearEnemy;
            newBot.Init(botBehavior, botType);
            m_Enemies.Add(newBot);
            return newBot;
        }

        private Vector2 GetRandomPositionForEnemy()
        {
            Vector2 spawnPosition = GetRandomPosition();
            ;
            int locker = 30;

            while (!IsSpawnPossible(spawnPosition, false))
            {
                if (--locker == 0)
                {
                    spawnPosition = Vector2.zero;
                    Debug.LogWarning($"Set default enemy position");
                    break;
                }

                spawnPosition = GetRandomPosition();
            }

            return spawnPosition;
        }

        private void ClearEnemy(BotController botController)
        {
            m_Enemies.Remove(botController);

            if (m_Enemies.Count == 0)
            {
                DestroyedAllEnemies?.Invoke();
            }
        }

        private Vector2 GetRandomPosition()
        {
            float x = Random.Range(m_FieldForSpawn.min.x, m_FieldForSpawn.max.x);
            float y = Random.Range(m_FieldForSpawn.min.y, m_FieldForSpawn.max.y);

            Vector2 spawnPos = new Vector2(x, y);
            return spawnPos;
        }

        private bool IsSpawnPossible(Vector2 placeToSpawn, bool playerCheck)
        {
            const int minAllowableDistance = 10;
            float distance;

            foreach (BotController enemy in m_Enemies)
            {
                if (enemy != null && !IsDistanceSufficient(enemy.transform.position, placeToSpawn)) /// Todo
                    return false;
            }

            if (playerCheck && !IsDistanceSufficient(m_Player.transform.position, placeToSpawn))
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

        private void SetSpawnField()
        {
            SpriteRenderer fieldRender = GetComponent<SpriteRenderer>();
            SpriteRenderer borderRenderer = m_Borders[0].GetComponent<SpriteRenderer>();
            Bounds fullBounds = fieldRender.bounds;
            float borderWidth = Mathf.Min(borderRenderer.bounds.size.x, borderRenderer.bounds.size.y);

            m_FieldForSpawn = new Bounds(Vector2.zero,
                new Vector2(fullBounds.max.x - fullBounds.min.x - borderWidth,
                    fullBounds.max.y - fullBounds.min.y - borderWidth));
        }

        public event Action DestroyedAllEnemies;
    }
}