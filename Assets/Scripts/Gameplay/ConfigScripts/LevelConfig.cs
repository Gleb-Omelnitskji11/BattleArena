using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public LevelModel[] LevelModels = Array.Empty<LevelModel>();

    public LevelModel GetDefaultLevel()
    {
        return LevelModels[0];
    }
}

[Serializable]
public class LevelModel
{
    public RaceType PlayerRace;
    public BotLevelModel[] Enemies = Array.Empty<BotLevelModel>();
    public BotLevelModel[] Allies = Array.Empty<BotLevelModel>();
}

[Serializable]
public class BotLevelModel
{
    public RaceType RaceType;
    public int Amount;
}