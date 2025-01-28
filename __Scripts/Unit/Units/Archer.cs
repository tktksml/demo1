using FTK.GamePlayLib.UnitLib.AttackLib;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class Archer : Warrior
    {
        [field: Header("Require Components")]
        [field: SerializeField] public Bullet BulletPrefab { get; set; } = null;







        public override AttackModel InitAttackModel()
        {
            return new RangeAttack(this, new RangeAttackSettings(AttackRange, AgroRange, AttackSpeed, AttackDamage)
            {
                BulletPrefab = BulletPrefab
            });
        }
    }
}