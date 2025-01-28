using System;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class WaitJob : Job
    {
        public WaitJob(float waitSec, Action onWaited)
        {
            this.waitSec = waitSec;
            this.onWaited = onWaited;
        }


        private float waitSec;
        private Action onWaited;








        protected override void OnJobStarted()
        {
            base.OnJobStarted();
            Unit.StopMoving();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();


            waitSec -= Time.deltaTime;
            if (waitSec <= 0)
            {
                onWaited?.Invoke();
                Unit.FindJob();
                return;
            }
        }
    }
}