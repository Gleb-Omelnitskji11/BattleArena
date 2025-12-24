using System;
using System.Collections.Generic;
using System.Linq;
using Game.Enemy;
using Game.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Tanks
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_Borders;

        private Bounds m_FieldForSpawn;

        public void Awake()
        {
            SetSpawnField();
        }

        public Vector2 GetRandomPosition()
        {
            float x = Random.Range(m_FieldForSpawn.min.x, m_FieldForSpawn.max.x);
            float y = Random.Range(m_FieldForSpawn.min.y, m_FieldForSpawn.max.y);

            Vector2 spawnPos = new Vector2(x, y);
            return spawnPos;
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
    }
}