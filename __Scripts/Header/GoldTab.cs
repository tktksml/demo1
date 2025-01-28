using KyokoLib.ServiceLib;
using FTK.GamePlayLib;
using UnityEngine;
using TMPro;


namespace FTK.UILib.HeaderLib
{
    public class GoldTab : MonoBehaviour, IInitable
    {
        [Header("Require Components")]
        [SerializeField] private TextMeshProUGUI goldText = null;








        public void PreInit() { SetGoldText(GameController.PlayerGold); }
        private void OnDestroy() { UnSubscribeEvent(); }


        public void SubscribeEvent() { GameController.OnPlayerGoldChanged += OnPlayerGoldChanged; }
        public void UnSubscribeEvent() { GameController.OnPlayerGoldChanged -= OnPlayerGoldChanged; }


        private void OnPlayerGoldChanged(int newPlayerGold) { SetGoldText(newPlayerGold); }
        private void SetGoldText(int goldCount) { goldText.text = $"{goldCount}"; }
    }
}