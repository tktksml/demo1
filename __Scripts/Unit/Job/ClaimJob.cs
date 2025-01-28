using FTK.GamePlayLib.MapLib.BuildingsLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class ClaimJob : Job
    {
        public ClaimJob(IClaimable targetClaimable) { this.targetClaimable = targetClaimable; }
        private IClaimable targetClaimable;








        protected override void OnJobStarted()
        {
            base.OnJobStarted();


            Unit.StopMoving();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();


            //Захватываем
            targetClaimable.OnClaiming(Time.deltaTime, Unit.Team);


            //Если мы полностью захватили объект, ищем новую работу для юнита
            if (targetClaimable.IsFullyClaimed)
                Unit.FindJob();
        }
        public override bool IsJobInteractable(Job newInteractedJob)
        {
            if (targetClaimable == null)
                return true;
            if (targetClaimable.IsFullyClaimed)
                return true;


            //Только атака может быть приоритетнее захвата
            if (newInteractedJob is FollowForAttackJob)
                return true;
            if (newInteractedJob is AttackJob)
                return true;


            return false;
        }
    }
}