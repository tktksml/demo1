using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class Healer : Warrior
    {
        [Header("Healer Require Components")]
        [SerializeField] private GameObject healParticles = null;


        [Header("Healer Settings")]
        [SerializeField, Range(0, 1)] private float healPercent = 0.1f;
        [SerializeField] private float healTime = 1f;








        public override void SubscribeEvent()
        {
            base.SubscribeEvent();


            OnUnitDamaged += OnUnitDamagedEvent;
        }
        public override void UnSubscribeEvent()
        {
            base.UnSubscribeEvent();


            OnUnitDamaged -= OnUnitDamagedEvent;
        }
        private void OnUnitDamagedEvent(Unit unit)
        {
            if (unit == this)
                return;


            //Если союзный юнит получил урон то пытаемся найти новую работу
            if (unit.Team.IsFriend(Team))
                AgroVisor.IsNotNull()?.ReTrigger();
        }


        public override void OnAgroVisorTriggered(GameObject triggeredObject)
        {
            base.OnAgroVisorTriggered(triggeredObject);


            //Если к нам подошел союзник то пытаемся его вылечить если у него мало хп
            if (triggeredObject.TryGetComponent(out IHealthable healthable))
            {
                if (healthable is ITeamable teamable && teamable.Team.IsFriend(Team))
                {
                    if (healthable.IsDamaged())
                        SetJob(new HealJob(healthable, healTime, healPercent, healParticles));
                }
            }
        }
    }
}