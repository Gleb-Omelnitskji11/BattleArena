using System;
using System.Linq;
using Game.Bullet;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Scriptable Objects/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField]
    private ProjectileConfigModel[] m_Projectiles;
    
    public ProjectileConfigModel GetCharacterModel(ProjectileType projectileType)
    {
        return m_Projectiles.FirstOrDefault(x => x.ProjectileType == projectileType);
    }
}

[Serializable]
public class ProjectileConfigModel
{
    public Projectile ProjectilePrefab;
    public float Speed;
    public ProjectileType ProjectileType;
}
