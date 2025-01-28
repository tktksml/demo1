using KyokoLib.ServiceLib;
using FTK.GamePlayLib.UnitLib;
using System.Collections.Generic;
using UnityEngine;


namespace FTK.UILib.CardLib
{
    public class CardController : MonoBehaviour, IInitable
    {
        [Header("Require Components")]
        [SerializeField] private List<Card> cards = new List<Card>();








        public void PostInit()
        {
            cards[0].Init(typeof(Miner), 30);
            cards[1].Init(typeof(Warrior), 50);
            cards[2].Init(typeof(Archer), 50);
            cards[3].Init(typeof(Tank), 50);
            cards[4].Init(typeof(Healer), 50);
        }
    }
}