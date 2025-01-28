using FTK.GamePlayLib.MapLib.BuildingsLib;
using FTK.GamePlayLib.UnitLib;
using KyokoLib.ServiceLib;
using System.Collections;
using UnityEngine;


namespace FTK.GamePlayLib.AILib
{
    public class AIController : MonoBehaviour, IInitable
    {
        [SerializeField] private float warriorSpawnCooldown = 10f;








        public void PostInit() { StartCoroutine(IWork()); }
        private IEnumerator IWork()
        {
            // UnitController.TryCreateUnit_<Warrior>(Team.EnemyTeam, out _);
            // UnitController.TryCreateUnit_<Warrior>(Team.EnemyTeam, out _);
            // UnitController.TryCreateUnit_<Warrior>(Team.EnemyTeam, out _);
            // UnitController.TryCreateUnit_<Warrior>(Team.EnemyTeam, out _);
            // Base.GetTeamBase(Team.TeamFlag.Enemy).ToggleAttackMode();


            UnitController.TryCreateUnit_<Miner>(Team.EnemyTeam, out _);
            yield return new WaitForSeconds(10f);
            UnitController.TryCreateUnit_<Miner>(Team.EnemyTeam, out _);


            yield return new WaitForSeconds(10f);
            int spawnedWarrior = 0;
            while (true)
            {
                yield return new WaitForSeconds(warriorSpawnCooldown);
                UnitController.TryCreateUnit_<Warrior>(Team.EnemyTeam, out var spawnedUnit);
                if (spawnedWarrior++ == 5)
                    Base.GetTeamBase(Team.TeamFlag.Enemy).ToggleAttackMode();
            }
        }
    }
}