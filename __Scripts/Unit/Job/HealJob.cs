using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class HealJob : Job
    {
        public HealJob(IHealthable allyHealthable, float healTime, float healPercent, GameObject healParticlesPrefab)
        {
            this.allyHealthable = allyHealthable;
            this.healTime = healTime;
            this.healPercent = healPercent;
            this.healParticlesPrefab = healParticlesPrefab;


            ResetHealTimer();
        }
        private readonly IHealthable allyHealthable;
        private readonly float healTime;
        private readonly float healPercent;
        private readonly GameObject healParticlesPrefab = null;

        private float currentHealTimer = 0;








        protected override void OnJobStarted()
        {
            base.OnJobStarted();


            //Перестаем двигаться, мы хилим
            Unit.StopMoving();
        }
        public override bool IsJobInteractable(Job newInteractedJob)
        {
            //Если тот кого мы хотим захилить мертв - то и хилить мы не можем
            if (allyHealthable.IsDead())
                return true;


            //Если мы полностью выхилили союзника - то можем завершать работу
            if (allyHealthable.IsDamaged() == false)
                return true;


            //Если жертва хила убегает, то не хилим ее
            if (Vector3.Distance(allyHealthable.TransformCache.position, Unit.TransformCache.position) > Unit.AgroRange * 1.1f)
                return true;


            //Вроде нет ничего важнее хила
            return false;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();


            //Ждем хила
            currentHealTimer -= Time.deltaTime;
            if (currentHealTimer <= 0)
            {
                //Обновляем таймер
                ResetHealTimer();


                int healValue = Mathf.CeilToInt(allyHealthable.MaxHealth * healPercent);
                allyHealthable.AddHeal(healValue);


                //Спауним партиклы
                var healParticles = Object.Instantiate(healParticlesPrefab, allyHealthable.TransformCache);
                Object.Destroy(healParticles, 3);


                //Проверяем можем ли мы завершить работу
                if (IsJobInteractable(null))
                {
                    Unit.FindJob();
                    return;
                }
            }
        }


        private void ResetHealTimer() { currentHealTimer = healTime; }
    }
}