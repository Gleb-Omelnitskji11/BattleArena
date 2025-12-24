using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Enemy
{
    public class BotController : MonoBehaviour
    {
        // [SerializeField]
        // private Transform m_BulletSpawnPoint;
        //
        // [SerializeField]
        // private SpriteRenderer m_TeamHightlight;
        //
        // private Color m_PlayerColor = Color.yellow;
        // private Color m_EnemyColor = Color.red;
        //
        // private const string AllyTag = "Ally";
        // private const string EnemyTag = "Enemy";
        //
        // private string m_CurrentTag;
        //
        // private IBotBehavior m_BotBehavior;
        // private BotType m_BotType;
        //
        // private bool m_DoBehavior;
        //
        // public void Init(IBotBehavior botBehavior, BotType botType)
        // {
        //     m_BotType = botType;
        //     m_BotBehavior = botBehavior;
        //     m_BotBehavior.SetTransform(transform);
        //     m_BotBehavior.SetBehaviorData();
        //     
        //     m_CurrentTag= gameObject.tag;
        //
        //     m_DoBehavior = true;
        // }
        //
        // private void Update()
        // {
        //     if (!m_DoBehavior) return;
        //
        //     m_BotBehavior.DoState();
        // }
        //
        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     if (m_CurrentTag.Equals(EnemyTag) && collision.gameObject.tag.Equals("PlayerBullet"))
        //     {
        //         DeleteTank();
        //         return;
        //     }
        //
        //     m_BotBehavior.OnCollisionEnter(collision.gameObject.tag);
        // }
        //
        // // private void OnCollisionStay2D(Collision2D collision)
        // // {
        // //     m_EnemyBehavior.OnCollisionStay(collision.gameObject.tag);
        // // }
        //
        // private void Die()
        // {
        // }
        //
        // private void DeleteTank()
        // {
        //     m_DoBehavior = false;
        //     gameObject.SetActive(false);
        //     Destroying?.Invoke(this);
        //     Destroy(gameObject);
        // }
        //
        // public event Action<BotController> Destroying;
    }
}