using KyokoLib.ServiceLib;
using System;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib.AttackLib
{
    public class BulletController : MonoBehaviour, IInitable
    {
        private static BulletController self = null;


        [Header("Require Components")]
        [SerializeField] private Transform bulletContainer = null;








        public void CoreInit() { self = this; }


        public static Bullet CreateBullet_(Bullet bulletPrefab, Transform from, Transform to, Action onReachedEnemy) => self.IsNotNull()?.CreateBullet(bulletPrefab, from, to, onReachedEnemy);
        private Bullet CreateBullet(Bullet bulletPrefab, Transform from, Transform to, Action onReachedEnemy)
        {
            //Создаем пулю
            var bullet = Instantiate(bulletPrefab, from.position, Quaternion.identity, bulletContainer);
            //Инитим пулю
            bullet.Init(from, to, onReachedEnemy);
            return bullet;
        }
    }
}