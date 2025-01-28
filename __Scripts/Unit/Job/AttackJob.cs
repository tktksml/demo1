namespace FTK.GamePlayLib.UnitLib
{
    public class AttackJob : Job
    {
        public AttackJob(IHealthable enemyHealthable) { this.enemyHealthable = enemyHealthable; }
        private IHealthable enemyHealthable;


        private float currentAttackTimer = 0;








        public override bool IsJobInteractable(Job newInteractedJob)
        {
            //Никакая задача не может быть приоритетнее чем атака, если конечно враг mou shinde iru
            if (enemyHealthable.IsDead())
                return true;


            //Хил важнее атаки
            if (newInteractedJob is HealJob)
                return true;


            return false;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();


            //Если враг умер, то юнит должен закончить свою работу и найти что-то новое
            if (enemyHealthable.IsDead())
            {
                Unit.FindJob();
                return;
            }


            // //Бежим к врагу если он далеко (нет не бежим, зачем, у нас есть триггер когда юнит рядом с врагом)
            // if (Unit.AttackModel.IsNeedToMoveToEnemy(enemyHealthable.TransformCache))
            // {
            //     Unit.SetDestination(enemyHealthable.GetPosition());
            //     return;
            // }


            Unit.StopMoving();


            //Таймер атаки
            if (currentAttackTimer <= 0)
            {
                //Восстанавливаем таймер
                currentAttackTimer = 1;
                //Бьем по врагу
                Unit.AttackModel.Attack(enemyHealthable);
                //Передаем информацию, что мы ударили
                Unit.OnAttack();
            }


            //Откатываем атаку
            Unit.AttackModel.OnCooldown(ref currentAttackTimer);
        }
    }
}