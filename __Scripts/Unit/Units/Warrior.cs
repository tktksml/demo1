using FTK.GamePlayLib.MapLib.BuildingsLib;
using FTK.GamePlayLib.MapLib;
using FTK.GamePlayLib.UnitLib.AttackLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class Warrior : Unit
    {
        private Cell unitCell = null;








        public override bool CanBeCreated(Team team)
        {
            //Воина можно призвать только если есть свободные для него ячейки
            return Map.GetAvailableUnitCell_(this, team) != null;
        }


        public override void SubscribeEvent()
        {
            base.SubscribeEvent();


            Base.OnAttackModeToggled += OnAttackModeChanged;
        }
        public override void UnSubscribeEvent()
        {
            base.UnSubscribeEvent();


            Base.OnAttackModeToggled -= OnAttackModeChanged;
        }


        public override void FindJob()
        {
            base.FindJob();


            if (GetUnitBase().IsAttackMode)
            {
                //Если сейчас режим атаки - бежим на вражескую башню
                var enemyBase = Base.GetTeamBase(Team.GetMyEnemy());
                //Бежим к вражеской башне
                SetJob(new MoveJob(enemyBase.transform.position));
            }
            else
            {
                //Если сейчас режим защиты, ищем себе точку защиты
                if (unitCell == null)
                {
                    unitCell = Map.GetAvailableUnitCell_(this, Team);
                    //Сохраняеи информацию о том, что теперь ячейка принадлежит этому воину
                    unitCell.SetUnit(this);
                }


                //Бежим к ячейке
                SetJob(new MoveJob(unitCell.transform.position));
            }
        }
        public override AttackModel InitAttackModel() { return new MeleeAttack(this, new AttackSettings(AttackRange, AgroRange, AttackSpeed, AttackDamage)); }


        /// <summary>
        /// Attached to Visor Event
        /// </summary>
        /// <param name="triggeredObject"></param>
        public virtual void OnAgroVisorTriggered(GameObject triggeredObject)
        {
            //Если воин видит того кого можно атаковать - бежим на него
            if (triggeredObject.TryGetComponent(out IHealthable healthable))
            {
                if (healthable is ITeamable teamable && teamable.Team.IsEnemy(Team))
                    SetJob(new FollowForAttackJob(healthable.TransformCache));
            }


            //Если объект можно захватить, бежим захватывать его
            if (triggeredObject.TryGetComponent(out IClaimable claimable) && claimable.IsFullyClaimed == false)
                SetJob(new FollowForAttackJob(claimable.TransformCache));
        }
        /// <summary>
        /// Attached to Visor Event
        /// </summary>
        /// <param name="triggeredObject"></param>
        public virtual void OnAttackVisorTriggered(GameObject triggeredObject)
        {
            //Если воин подошел к объекту который можно атаковать и это враг - то атакуем
            if (triggeredObject.TryGetComponent(out IHealthable healthable))
            {
                if (healthable is ITeamable teamable && teamable.Team.IsEnemy(Team))
                    SetJob(new AttackJob(healthable));
            }


            if (triggeredObject.TryGetComponent(out IClaimable claimable))
            {
                if (claimable.IsFullyClaimed == false)
                {
                    SetJob(new ClaimJob(claimable));
                }
                else
                {
                    //Если юнит бежал захватывать, но пока бежал объект уже захватили, нужно чтобы юнит ншаел новую работу
                    if (CurrentJob is FollowForAttackJob followJob && followJob.FollowObject == claimable.TransformCache)
                    {
                        CurrentJob = null;
                        FindJob();
                    }
                }
            }
        }


        public void OnAttackModeChanged(Base _base, bool isCurrentModeAttack)
        {
            //Игнорируем вражеские здания
            if (_base.Team.IsEnemy(Team))
                return;


            FindJob();
        }
    }
}