using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Enemy
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "ScriptableObjects/EnemiesConfig", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField]
        private EnemyModel[] m_EnemyModels;

        public IBotBehavior GetEnemyBehavior(BotType botType)
        {
            switch (botType)
            {
                case BotType.SimpleEnemy:
                    return new SimpleBot();
                default:
                    throw new ArgumentOutOfRangeException(nameof(botType), botType, null);
            }
        }

        public EnemyModel GetEnemyModel(BotType botType)
        {
            return m_EnemyModels.FirstOrDefault(x => x.BotType == botType);
        }
    }

    [Serializable]
    public class EnemyModel
    {
        [FormerlySerializedAs("EnemyType")]
        public BotType BotType;
        public BotController TankPrefab;
    }
}

public enum BotType
{
    SimpleEnemy
}
