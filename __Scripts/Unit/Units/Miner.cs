using FTK.GamePlayLib.MapLib.BuildingsLib;
using FTK.GamePlayLib.UnitLib.AttackLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class Miner : Unit
    {
        [Header("Worker Settings")]
        [SerializeField] private float miningTime = 3f;
        [SerializeField] private int goldPerRun = 10;


        /// <summary>
        /// Шахта в которой сейчас работает рабочий
        /// </summary>
        private Mine currentWorkingMine = null;
        /// <summary>
        /// Есть ли золото у рабочего
        /// </summary>
        private bool isCarryingGold = false;








        public override bool CanBeCreated(Team team)
        {
            //Рабочих всегда можно создать
            return true;
        }


        public override void FindJob()
        {
            base.FindJob();


            //Если у рабочего есть золото, то его задача принести его на базу
            if (isCarryingGold)
            {
                //Находим базу рабочего
                var unitBase = GetUnitBase();
                //Бежим на базу
                SetJob(new MoveJob(unitBase.transform.position));
                return;
            }


            //У рабочего нет золота, значит бежим его добывать, находим подходящую шахту, если ее нет
            if (currentWorkingMine == null)
            {
                currentWorkingMine = Mine.GetAvailableMineForWorker(this);
                //Подписываемся к ней, теперь мы всегда работаем с ней
                currentWorkingMine.AddWorker(this);
            }


            //Бежим к шахте
            SetJob(new MoveJob(currentWorkingMine.transform.position));
        }


        public override AttackModel InitAttackModel() { return new MeleeAttack(this, new AttackSettings(AttackRange, AgroRange, AttackSpeed, AttackDamage)); }


        /// <summary>
        /// Attached to Visor Event
        /// </summary>
        /// <param name="triggeredObject"></param>
        public void OnVisorTriggered(GameObject triggeredObject)
        {
            //Проверяем коснулcя ли рабочий шахты
            if (triggeredObject.TryGetComponent(out Mine mine))
            {
                //Проверяем шел ли рабочий до шахты чтобы добыть золото
                if (CurrentJob is MoveJob moveJob && moveJob.TargetPosition == mine.transform.position)
                {
                    //Добываем
                    SetJob(new WaitJob(miningTime, () =>
                    {
                        //Сохраняем флаг что шахтер получил золото
                        isCarryingGold = true;
                    }));
                    return;
                }
            }


            if (triggeredObject.TryGetComponent(out Base _base))
            {
                //Проверяем шел ли рабочий до базы вместе с золотом
                if (CurrentJob is MoveJob moveJob && moveJob.TargetPosition == _base.transform.position)
                {
                    //Убираем фраг, что у рабочего есть золото
                    isCarryingGold = false;


                    //Добавляем золото на баланс если это союзный рабочий
                    if (Team.IsFriend(Team.TeamFlag.Player))
                        GameController.AddPlayerGold_(goldPerRun);


                    //Ищем следующую работу
                    FindJob();
                    return;
                }
            }
        }
    }
}