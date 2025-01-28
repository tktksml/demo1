using FTK.GamePlayLib.UnitLib.AttackLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class Tower : Unit
    {
        [field: SerializeField] public Bullet BulletPrefab { get; private set; } = null;








        public override bool CanBeCreated(Team team) { return true; }
        public override AttackModel InitAttackModel()
        {
            return new TowerAttack(this, new RangeAttackSettings(AttackRange, AgroRange, AttackSpeed, AttackDamage)
            {
                BulletPrefab = BulletPrefab
            });
        }


        /// <summary>
        /// Attached to Visor Event
        /// </summary>
        /// <param name="triggeredObject"></param>
        public void OnVisorForFightingWithEnemyTriggered(GameObject triggeredObject)
        {
            if (triggeredObject == null)
                return;


            //Если воин подошел к объекту который можно атаковать и это враг - то атакуем
            if (triggeredObject.TryGetComponent(out IHealthable healthable))
            {
                if (healthable is ITeamable teamable && teamable.Team.IsEnemy(Team))
                    SetJob(new AttackJob(healthable));
            }
        }
    }
}