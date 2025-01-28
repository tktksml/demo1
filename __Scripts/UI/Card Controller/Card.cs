using System;
using FTK.GamePlayLib.UnitLib;
using FTK.GamePlayLib;
using UnityEngine;


namespace FTK.UILib.CardLib
{
    public class Card : MonoBehaviour
    {
        private Type unitType = null;
        private int unitCost = 0;








        public void Init<T>(T unitSpawnType, int unitCost) where T : Type
        {
            unitType = unitSpawnType;
            this.unitCost = unitCost;
        }
        /// <summary>
        /// Attached to button
        /// </summary>
        public void Spawn()
        {
            if (GameController.PlayerGold >= unitCost)
            {
                if (UnitController.TryCreateUnit_(unitType, Team.PlayerTeam, out var unit))
                {
                    //Забираем деньги у игрока
                    GameController.AddPlayerGold_(-unitCost);
                }
            }
        }
    }
}