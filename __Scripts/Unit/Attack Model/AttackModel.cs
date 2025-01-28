using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.AttackLib
{
    public class AttackSettings
    {
        public AttackSettings(float attackRange, float agroRange, float attackSpeed, int attackDamage)
        {
            AttackRange = attackRange;
            AgroRange = agroRange;
            AttackSpeed = attackSpeed;
            AttackDamage = attackDamage;
        }
        public float AttackRange { get; set; }
        public float AgroRange { get; set; }
        public float AttackSpeed { get; set; }
        public int AttackDamage { get; set; }
    }
    public abstract class AttackModel
    {
        protected AttackModel(Unit unit) { Unit = unit; }


        public abstract AttackSettings BaseSettings { get; }
        public Unit Unit { get; }








        public abstract void Attack(IHealthable enemy);
        public abstract void OnCooldown(ref float currentCooldownTimer);
    }
    public abstract class AttackModel<TAttackSettings> : AttackModel where TAttackSettings : AttackSettings
    {
        public const float ATTACKSPEED_PER_SECOND = 100f;


        protected AttackModel(Unit unit, TAttackSettings settings) : base(unit) { Settings = settings; }
        public override AttackSettings BaseSettings => Settings;
        public TAttackSettings Settings { get; }








        public override void OnCooldown(ref float currentCooldownTimer) { currentCooldownTimer -= Time.deltaTime / (Settings.AttackSpeed / ATTACKSPEED_PER_SECOND); }
    }
}