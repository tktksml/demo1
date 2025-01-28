using KyokoLib.ServiceLib;
using KyokoLib.CoreLib;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace FTK.GamePlayLib.UnitLib
{
    public class UnitController : MonoBehaviour, IInitable
    {
        private static UnitController self = null;


        [Header("Require Components")]
        [SerializeField] private Transform unitContainer = null;
        [SerializeField] private List<Unit> unitPrefabs = new List<Unit>();


        [Header("Spawn Points")]
        [SerializeField] private Transform playerSpawnPoint = null;
        [SerializeField] private Transform enemySpawnPoint = null;








        public void CoreInit() { self = this; }


        public static bool TryCreateUnit_<T>(Team targetTeam, out T createdUnit) where T : Unit
        {
            if (self.IsNotNull() == null)
            {
                createdUnit = null;
                return false;
            }
            return self.TryCreateUnit(targetTeam, out createdUnit);
        }
        public bool TryCreateUnit<T>(Team targetTeam, out T createdUnit) where T : Unit
        {
            bool created = TryCreateUnit(typeof(T), targetTeam, out var createdUnit_);
            createdUnit = createdUnit_ as T;
            return created;
        }


        public static bool TryCreateUnit_(Type unitType, Team targetTeam, out Unit createdUnit)
        {
            if (self.IsNotNull() == null)
            {
                createdUnit = null;
                return false;
            }
            return self.TryCreateUnit(unitType, targetTeam, out createdUnit);
        }
        public bool TryCreateUnit(Type unitType, Team targetTeam, out Unit createdUnit)
        {
            //Находим префаб юнита по его типу
            var prefab = GetUnitPrefab(unitType);
            //Спрашиваем у юнита может ли он создасться, ведь у каждого юнита свои собсвтенные ограничения
            if (!prefab.CanBeCreated(targetTeam))
            {
                createdUnit = null;
                return false;
            }


            //Получаем спаунпоинт
            var unitSpawnPoint = targetTeam.IsFriend(Team.TeamFlag.Player) ? playerSpawnPoint.position : enemySpawnPoint.position;


            //Создаем юнита в контейнере
            createdUnit = Instantiate(prefab, unitSpawnPoint, Quaternion.identity, unitContainer);
            //Инитим созданный юнит
            createdUnit.Init(targetTeam);
            return true;
        }


        public static T GetUnitPrefab_<T>() where T : Unit => self.IsNotNull()?.GetUnitPrefab<T>();
        public T GetUnitPrefab<T>() where T : Unit => GetUnitPrefab(typeof(T)) as T;
        public Unit GetUnitPrefab(Type unitType)
        {
            var unitPrefab = unitPrefabs.Find(unitPrefab => unitPrefab.GetType() == unitType);
            if (unitPrefab != null)
                return unitPrefab;
            Core.Error($"Can't find unitPrefab with type '{unitType}', returning first prefab in pool: '{unitPrefabs[0]}'", nameof(UnitController), nameof(GetUnitPrefab));
            return unitPrefabs[0];
        }
    }
}