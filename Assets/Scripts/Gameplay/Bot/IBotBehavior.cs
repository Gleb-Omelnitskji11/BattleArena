using System.Collections;
using UnityEngine;

namespace Game.Enemy
{
    public interface IBotBehavior
    {
        public void SetTransform(Transform transform);
        public void SetBehaviorData();
        public void OnCollisionEnter(string objectTag);
        //public void OnCollisionStay(string objectTag);
        public void DoState();
    }
}