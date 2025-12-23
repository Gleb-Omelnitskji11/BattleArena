using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Bullet
{
    public class BulletSpawner : MonoBehaviour
    {
        [SerializeField]
        private Bullet m_BulletPrefab;

        private readonly List<Bullet> m_PooledBullets = new List<Bullet>();

        public static BulletSpawner Instance;
        private const float BulletSpeed = 15f;
        private const string PlayerBulletTag = "PlayerBullet";

        private void Awake()
        {
            Instance = this;
        }

        public void Clear()
        {
            foreach (var bullet in m_PooledBullets)
            {
                bullet.TurnOff();
            }
        }

        public void SpawnBullet(Transform startPoint)
        {
            var bullet = GetFreeBullet();
            var bulletTransform = bullet.transform;
            bulletTransform.position = startPoint.position;
            bulletTransform.rotation = startPoint.rotation;
            bullet.tag = PlayerBulletTag;
            bullet.StartMove(BulletSpeed);
        }

        private Bullet GetFreeBullet()
        {
            foreach (var bullet in m_PooledBullets)
            {
                if (!bullet.IsBusy)
                {
                    bullet.gameObject.SetActive(true);
                    return bullet;
                }
            }

            return CreateNew();
        }

        private Bullet CreateNew()
        {
            Bullet bullet = Instantiate(m_BulletPrefab);
            bullet.gameObject.SetActive(true);
            m_PooledBullets.Add(bullet);
            return bullet;
        }
    }
}