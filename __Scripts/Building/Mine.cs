using System;
using KyokoLib.ServiceLib;
using FTK.GamePlayLib.UnitLib;
using System.Collections.Generic;
using System.Linq;
using KyokoLib.CoreLib;
using UnityEngine;


namespace FTK.GamePlayLib.MapLib.BuildingsLib
{
    public class Mine : MonoBehaviour, IInitable
    {
        private static List<Mine> Mines = new List<Mine>();


        private List<Miner> activeWorkers = new List<Miner>();


        /// <summary>
        /// К какой команде относится шахта
        /// </summary>
        [field: SerializeField] public Team Team { get; private set; } = new Team(Team.TeamFlag.Player);








        public void CoreInit() { Mines.Add(this); }
        private void OnDestroy() { Mines.Remove(this); }


        public void AddWorker(Miner miner)
        {
            if (miner.Team.IsEnemy(Team))
            {
                Core.Warning($"Can't add worker to mine, coz worker is enemy!");
                return;
            }
            activeWorkers.AddIfNotContains(miner);
        }
        public void RemoveWorker(Miner miner) { activeWorkers.Remove(miner); }


        /// <summary>
        /// Вовзращает шахту для рабочего
        /// </summary>
        /// <returns></returns>
        public static Mine GetAvailableMineForWorker(Miner targetMiner)
        {
            if (Mines.Count <= 0)
                throw new InvalidOperationException($"Can't get mine for worker, coz count of list '{nameof(Mines)}' is 0");


            //Проверяем вдруг рабочий итак сейчас работает на какой-то шахте
            var alreadyWorkingMine = Mines.Find(mine => mine.activeWorkers.Contains(targetMiner));
            if (alreadyWorkingMine != null)
                return alreadyWorkingMine;


            //Сначала нужно найти союзные шахты, а потом отсортировать по шахтам с меньшим кол-вом рабочих
            return Mines.FindAll(mine => mine.Team.IsFriend(targetMiner.Team)).OrderBy(mine => mine.activeWorkers.Count()).First();
        }
    }
}