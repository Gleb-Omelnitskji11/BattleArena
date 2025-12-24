using Game.Bullet;
using Systems.InputService;
using TowerDefence.Core;
using TowerDefence.Game;
using TowerDefence.Systems;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        // [SerializeField]
        // private Transform m_BulletSpawnPoint;
        //
        // private float m_MoveSpeed = 30;
        // private float m_FireDelay = 1f;
        //
        // private IInputService m_PlayerInputController;
        // private IEventBus m_EventBus;
        // private float m_CurrentDelay;
        //
        //
        // public Direction CurrentDirection { get; private set; } = Direction.Up;
        // public Vector2 MoveVector { get; private set; } = Vector2.up;
        // private Camera m_Camera;
        // private Vector3 m_TargetWorldPosition;
        // private bool m_IsMoving;
        //
        // private void Start()
        // {
        //     SetSystems();
        // }
        //
        // private void SetSystems()
        // {
        //     m_EventBus = Services.Get<IEventBus>();
        //     m_Camera = Camera.main;
        //
        //     if (Services.TryGet<IInputService>(out var inputService))
        //     {
        //         m_PlayerInputController = inputService;
        //     }
        //     
        //     m_PlayerInputController.OnHold += OnHold;
        //     m_PlayerInputController.OnTap += OnTap;
        //     m_PlayerInputController.OnTouchMoved += OnTouchMoved;
        //     m_PlayerInputController.OnCancel += OnTouchCancelled;
        // }
        //
        // private void OnDestroy()
        // {
        //     if (m_PlayerInputController == null) return;
        //
        //     m_PlayerInputController.OnHold -= OnHold;
        //     m_PlayerInputController.OnTap -= OnTap;
        //     m_PlayerInputController.OnTouchMoved -= OnTouchMoved;
        //     m_PlayerInputController.OnCancel -= OnTouchCancelled;
        // }
        //
        // private void Update()
        // {
        //     if (!m_IsMoving)
        //         return;
        //
        //     Move();
        // }
        //
        // private void OnHold(Vector2 screenPos)
        // {
        //     UpdateDirection(screenPos);
        //     if(screenPos != Vector2.zero)
        //         m_IsMoving = true;
        //     else OnTouchCancelled();
        // }
        //
        // private void OnTouchMoved(Vector2 screenPos)
        // {
        //     UpdateDirection(screenPos);
        //     if(screenPos != Vector2.zero)
        //         m_IsMoving = true;
        //     else OnTouchCancelled();
        // }
        //
        // private void OnTouchCancelled()
        // {
        //     m_IsMoving = false;
        // }
        //
        // private void OnTap(Vector2 screenPos)
        // {
        //     UpdateDirection(screenPos);
        //     
        //     if(screenPos!= Vector2.zero)
        //         m_IsMoving = true;
        //     else OnTouchCancelled();
        //     Shoot();
        // }
        //
        // private void UpdateDirection(Vector2 screenPos)
        // {
        //     Vector3 worldPos = m_Camera.ScreenToWorldPoint(
        //         new Vector3(screenPos.x, screenPos.y, Mathf.Abs(m_Camera.transform.position.z))
        //     );
        //
        //     Vector2 delta = worldPos - transform.position;
        //
        //     if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        //     {
        //         if (delta.x > 0)
        //             SetDirection(Direction.Right);
        //         else
        //             SetDirection(Direction.Left);
        //     }
        //     else
        //     {
        //         if (delta.y > 0)
        //             SetDirection(Direction.Up);
        //         else
        //             SetDirection(Direction.Down);
        //     }
        // }
        //
        // private void SetDirection(Direction direction)
        // {
        //     CurrentDirection = direction;
        //
        //     switch (direction)
        //     {
        //         case Direction.Up:
        //             MoveVector = Vector2.up;
        //             transform.rotation = Quaternion.Euler(0, 0, 0);
        //             break;
        //         case Direction.Down:
        //             MoveVector = Vector2.down;
        //             transform.rotation = Quaternion.Euler(0, 0, 180);
        //             break;
        //         case Direction.Left:
        //             MoveVector = Vector2.left;
        //             transform.rotation = Quaternion.Euler(0, 0, 90);
        //             break;
        //         case Direction.Right:
        //             MoveVector = Vector2.right;
        //             transform.rotation = Quaternion.Euler(0, 0, -90);
        //             break;
        //     }
        // }
        //
        // // private void Update()
        // // {
        // //     Move();
        // //     Shoot();
        // // }
        //
        // private void Move()
        // {
        //     Vector3 move = new Vector3(MoveVector.x, MoveVector.y, 0);
        //     transform.position += move * m_MoveSpeed * Time.deltaTime;
        // }
        //
        // private void Shoot()
        // {
        //     if (m_CurrentDelay > 0)
        //     {
        //         m_CurrentDelay -= Time.deltaTime;
        //         return;
        //     }
        //
        //     m_CurrentDelay = m_FireDelay;
        //     BulletSpawner.Instance.SpawnBullet(m_BulletSpawnPoint, ProjectileType.Bullet);
        // }
        //
        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     if (collision.gameObject.tag.Equals("Enemy"))
        //     {
        //         //OnCollisionWithEnemy();
        //     }
        // }
        //
        // private void OnCollisionWithEnemy()
        // {
        //     m_EventBus.Publish(new GameOverEvent());
        //     gameObject.SetActive(false);
        // }
    }
}