using JetBrains.Annotations;


namespace FTK.GamePlayLib.UnitLib
{
    /// <summary>
    /// Работа юнита
    /// </summary>
    public abstract class Job
    {
        public Unit Unit { get; private set; }








        public void Init(Unit unit)
        {
            Unit = unit;


            OnJobStarted();
        }
        /// <summary>
        /// Начало работы
        /// </summary>
        protected virtual void OnJobStarted() { }
        /// <summary>
        /// Может ли текущая работа быть заменена другой?
        /// </summary>
        /// <param name="newInteractedJob"></param>
        public virtual bool IsJobInteractable([CanBeNull] Job newInteractedJob) { return true; }
        /// <summary>
        /// Вызывается когда резко появилась новая работа в то время как текущая работа еще не завершилась
        /// </summary>
        /// <param name="newInteractedJob"></param>
        public virtual void OnJobInteracted(Job newInteractedJob) { }


        /// <summary>
        /// Update MonoBehavior
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// FixedUpdate MonoBehavior
        /// </summary>
        public virtual void OnFixedUpdate() { }
        /// <summary>
        /// Destroy MonoBehavior
        /// </summary>
        public virtual void OnDestroy() { }
    }
}