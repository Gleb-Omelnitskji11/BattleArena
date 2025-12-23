using System;
using UnityEngine.Serialization;

namespace Game.LevelSystem
{
    [Serializable]
    public class EnemyLevelModel
    {
        [FormerlySerializedAs("EnemyType")]
        public BotType BotType;
        public int Amount;
    }
}