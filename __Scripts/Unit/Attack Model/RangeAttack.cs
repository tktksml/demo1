using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.AttackLib
{
    public class RangeAttackSettings : AttackSettings
    {
        public Bullet BulletPrefab { get; set; }
        public RangeAttackSettings(float attackRange, float agroRange, float attackSpeed, int attackDamage) : base(attackRange, agroRange, attackSpeed, attackDamage) { }
    }
    public class RangeAttack : AttackModel<RangeAttackSettings>
    {
        public RangeAttack(Unit unit, RangeAttackSettings settings) : base(unit, settings) { }








        public override void Attack(IHealthable enemy)
        {
            //Создаем пулю
            BulletController.CreateBullet_(Settings.BulletPrefab, Unit.TransformCache, enemy.TransformCache, () =>
            {
                //Когда пуля долетит - дамажим
                enemy.SetDamage(Settings.AttackDamage);
            });
        }
    }
}