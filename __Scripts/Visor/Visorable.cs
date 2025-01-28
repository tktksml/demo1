using System;
using KyokoLib.CoreLib;
using UnityEngine;


namespace FTK.GamePlayLib
{
    public class Visorable : MonoBehaviour
    {
        [field: SerializeField] public GameObject Target { get; private set; } = null;








        private void Awake()
        {
            if (gameObject.CompareTag(Visor.VISOR_TAG) == false)
            {
                Core.Error($"Object '{name}' has '{nameof(Visorable)}' component, but don't have '{Visor.VISOR_TAG}' tag, applying via script", nameof(Visorable));
                gameObject.tag = Visor.VISOR_TAG;
            }
        }
    }
}