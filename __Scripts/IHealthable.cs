using UnityEngine;


namespace FTK.GamePlayLib
{
    public interface IHealthable
    {
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public Transform TransformCache { get; }








        public void SetDamage(int damageValue)
        {
            //Если мы итак умерли, нет смысла дальше бить
            if (damageValue > 0 && IsDead())
                return;


            //Забираем хп
            CurrentHealth = Mathf.Clamp(CurrentHealth - damageValue, 0, MaxHealth);
            //Вызываем эвент
            OnHealthDecreased();
            //Если умерли - тоже вызываем эвент
            if (IsDead())
                OnHealthBelowZero();
        }
        public void AddHeal(int healValue)
        {
            //Нельзя хилить мертвого
            if (IsDead())
                return;


            //Добавляем хп
            CurrentHealth = Mathf.Clamp(CurrentHealth + healValue, 0, MaxHealth);
        }


        public bool IsDead() => CurrentHealth <= 0;
        public bool IsDamaged() => CurrentHealth < MaxHealth;


        public void OnHealthDecreased();
        public void OnHealthBelowZero();


        public Vector3 GetPosition();
    }
}