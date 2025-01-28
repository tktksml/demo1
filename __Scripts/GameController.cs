using System;
using System.Linq;
using FTK.GamePlayLib.UnitLib;
using KyokoLib.ServiceLib;
using Unity.VisualScripting;
using UnityEngine;


namespace FTK.GamePlayLib
{
    public class GameController : MonoBehaviour, IInitable
    {
        private static event Action<int> onPlayerGoldChanged = null;
        public static event Action<int> OnPlayerGoldChanged
        {
            add
            {
                if (onPlayerGoldChanged == null || !onPlayerGoldChanged.GetInvocationList().Contains(value))
                    onPlayerGoldChanged += value;
            }
            remove
            {
                if (onPlayerGoldChanged != null && onPlayerGoldChanged.GetInvocationList().Contains(value))
                    onPlayerGoldChanged -= value;
            }
        }


        private static GameController self = null;


        public static int PlayerGold { get; private set; } = 0;








        public void CoreInit()
        {
            self = this;


            //Сбрасываем счетчик монет у игрока
            PlayerGold = 0;
        }
        public void PostInit() { UnitController.TryCreateUnit_<Miner>(Team.PlayerTeam, out _); }


        public static void AddPlayerGold_(int value) => self.IsNotNull()?.AddPlayerGold(value);
        public void AddPlayerGold(int value)
        {
            PlayerGold = Mathf.Max(PlayerGold + value, 0);
            onPlayerGoldChanged?.Invoke(PlayerGold);
        }
    }
}