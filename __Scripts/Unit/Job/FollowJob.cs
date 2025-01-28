using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class FollowJob : Job
    {
        public FollowJob(Transform followObject) { FollowObject = followObject; }


        public Transform FollowObject { get; } = null;








        protected override void OnJobStarted()
        {
            //Направляем юнита к точке
            Unit.SetDestination(FollowObject.position);
        }
        public override void OnUpdate()
        {
            if (FollowObject.IsNotNull() == null)
            {
                Unit.FindJob();
                return;
            }


            Unit.SetDestination(FollowObject.position);
        }
    }
    public class FollowForAttackJob : FollowJob
    {
        public FollowForAttackJob(Transform followObject) : base(followObject) { }


        public override bool IsJobInteractable(Job newInteractedJob)
        {
            //Атака может быть приоритетнее чем эта работа
            if (newInteractedJob is AttackJob)
                return true;


            //Хил приоритетнее
            if (newInteractedJob is HealJob)
                return true;


            //Захват приоритетнее
            if (newInteractedJob is ClaimJob)
                return true;


            //Ну или если объекта нет
            if (FollowObject == null)
                return true;


            return false;
        }
    }
}