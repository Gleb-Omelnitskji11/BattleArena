using Game.LevelSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public LevelModel[] LevelModels = new LevelModel[0];

    public LevelModel GetDefaultLevel()
    {
        return LevelModels[0];
    }
}
