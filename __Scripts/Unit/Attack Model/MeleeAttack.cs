using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.AttackLib
{
    public class MeleeAttack : AttackModel<AttackSettings>
    {
        public MeleeAttack(Unit unit, AttackSettings settings) : base(unit, settings) { }







        
        public override void Attack(IHealthable enemy) { enemy.SetDamage(Settings.AttackDamage); }
    }
}