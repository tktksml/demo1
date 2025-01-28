using DG.Tweening;
using KyokoLib.ExtensionLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.VisualLib
{
    public class UnitVisual : MonoBehaviour
    {
        public enum AnimationKey
        {
            Idle,
            Run,
            Attack,
            Hit
        }


        [SerializeField] private Animator animator = null;


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








        public void OnIdle() { PlayAnimation(AnimationKey.Idle); }
        public void OnStartRun() { PlayAnimation(AnimationKey.Run); }
        public void OnAttacked()
        {
            TransformCache.DOPunchScale(Vector3.one * 0.15f, 0.5f, vibrato: 1);
            // PlayAnimation(AnimationKey.Hit);
        }
        public void OnAttack() { SetTrigger(AnimationKey.Attack); }


        public void PlayAnimation(AnimationKey key) { animator.IsNotNull()?.Play(key.ToString()); }
        public void SetTrigger(AnimationKey key) { animator.IsNotNull()?.SetTrigger(key.ToString()); }
        public void Toggle(bool state) => gameObject.Toggle(state);
    }
}