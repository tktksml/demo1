using KyokoLib.ServiceLib;
using FTK.GamePlayLib.UnitLib;
using UnityEngine;


namespace FTK.GamePlayLib.MapLib
{
    [SelectionBase]
    public class Cell : MonoBehaviour, IInitable
    {
        [Header("Settings")]
        [field: SerializeField] public SerializeType<Unit> UnitTypeFilter { get; private set; } = new SerializeType<Unit>();


        public Unit CurrentUnit { get; private set; } = null;


        private Transform transformCache = null;
        public Transform TransformCache
        {
            get
            {
                if (transformCache == null)
                    transformCache = transform;
                return transformCache;
            }
        }








        private void OnDestroy() { UnSubscribeEvent(); }
        public void SubscribeEvent() { Unit.OnUnitDied += OnUnitDestroyed; }
        public void UnSubscribeEvent() { Unit.OnUnitDied += OnUnitDestroyed; }


        private void OnUnitDestroyed(Unit destroyedUnit)
        {
            //Если умер наш воин, то снимаем с него ссылку
            if (CurrentUnit == destroyedUnit)
                CurrentUnit = null;
        }


        public void SetUnit(Unit unit) { CurrentUnit = unit; }
        public void Destroy() { Utils.Destroy(gameObject); }
    }
}