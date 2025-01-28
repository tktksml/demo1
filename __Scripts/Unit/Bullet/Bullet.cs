using System;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.AttackLib
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")]
        [field: SerializeField] public float MoveSpeed { get; set; } = 1;


        private Transform target = null;
        private Action onReachedTarget = null;


        private Transform transformCache = null;
        public Transform TransformCache
        {
            get
            {
                if (transformCache == null)
                    transformCache = transform;
                return transformCache;
            }
        }








        public void Init(Transform from, Transform target, Action onReachedTarget)
        {
            this.target = target;
            this.onReachedTarget = onReachedTarget;
        }
        private void Update()
        {
            if (TransformCache.IsNotNull() == null || target.IsNotNull() == null)
            {
                Destroy();
                return;
            }


            TransformCache.position = Vector3.MoveTowards(TransformCache.position, target.position, MoveSpeed);
            if (TransformCache.position == target.position)
            {
                onReachedTarget?.Invoke();
                Destroy();
            }
        }
        private void Destroy() { Destroy(gameObject); }
    }
}