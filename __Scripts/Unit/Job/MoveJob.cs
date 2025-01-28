using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class MoveJob : Job
    {
        public MoveJob(Vector3 targetPosition) { TargetPosition = targetPosition; }


        public Vector3 TargetPosition { get; } = Vector3.zero;








        public override void OnUpdate()
        {
            base.OnUpdate();


            //Проверяем дошел ли юнит до своей точки
            var unitNavMesh = Unit.UnitNavMesh;
            if (!unitNavMesh.pathPending)
            {
                if (unitNavMesh.remainingDistance <= unitNavMesh.stoppingDistance)
                {
                    if (!unitNavMesh.hasPath || unitNavMesh.velocity.sqrMagnitude == 0f)
                        Unit.CurrentTeamVisual.OnIdle();
                }
            }
        }
        protected override void OnJobStarted()
        {
            //Направляем юнита к точке
            Unit.SetDestination(TargetPosition);
        }
    }
}