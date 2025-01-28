using KyokoLib.CoreLib;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine;


namespace FTK.GamePlayLib
{
    [RequireComponent(typeof(Rigidbody))]
    public class Visor : MonoBehaviour
    {
        public const string VISOR_TAG = "Visorable";


        [Header("Require Components")]
        [SerializeField] private Collider visorCollider = null;


        [Header("Events")]
        [SerializeField] private UnityEvent<GameObject> onVisorTriggeredTarget = new UnityEvent<GameObject>();
        [SerializeField] private UnityEvent onVisorTriggeredCustom = new UnityEvent();


        [NonSerialized] public List<Collider> TriggeredList = new List<Collider>();








        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(VISOR_TAG))
            {
                //Сохраняем информацию о том, что объект внутри действия
                TriggeredList.AddIfNotContains(other);


                onVisorTriggeredTarget?.Invoke(other.GetComponent<Visorable>()?.Target);
                onVisorTriggeredCustom?.Invoke();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            //Убираем объект если он вышел из зоны действия
            TriggeredList.Remove(other);
        }


        /// <summary>
        /// Заново прогоняет эвенты если объекты все еще в зоне действия
        /// </summary>
        public void ReTrigger()
        {
            for (int i = 0; i < TriggeredList.Count; i++)
            {
                if (TriggeredList[i] == null)
                {
                    TriggeredList.RemoveAt(i--);
                    continue;
                }
                //Тригерим эвент
                OnTriggerEnter(TriggeredList[i]);
                //Удаляем его из списка, т.к. уже затригерили
                // TriggeredList.RemoveAt(i--);
            }
        }


        public void UpdateRadius(float newRadius)
        {
            if (visorCollider is not SphereCollider sphereCollider)
            {
                Core.Error($"Can't change visor '{name}' radius, visor is '{visorCollider.GetType()}' and not SphereCollider");
                return;
            }
            sphereCollider.radius = newRadius;
        }
    }
}