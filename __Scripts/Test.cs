using KyokoLib.ServiceLib;
using FTK.GamePlayLib.MapLib.BuildingsLib;
using FTK.GamePlayLib.UnitLib;
using FTK.GamePlayLib;
using FTK;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Test : MonoBehaviour, IInitable
{
    private static Test self = null;








    public void CoreInit() { self = this; }


    /// <summary>
    /// Attached to button
    /// </summary>
    public void SpawnMiner()
    {
        if (GameController.PlayerGold >= 10)
        {
            //Создаем юнита
            if (UnitController.TryCreateUnit_<Miner>(Team.PlayerTeam, out _))
            {
                //Забираем деньги у игрока
                GameController.AddPlayerGold_(-10);
            }
        }
    }
    /// <summary>
    /// Attached to button
    /// </summary>
    public void SpawnWarrior()
    {
        if (GameController.PlayerGold >= 10)
        {
            //Создаем юнита
            if (UnitController.TryCreateUnit_<Warrior>(Team.PlayerTeam, out _))
            {
                //Забираем деньги у игрока
                GameController.AddPlayerGold_(-10);
            }
        }
    }
    /// <summary>
    /// Attached to button
    /// </summary>
    public void ToggleIsAttackMode() { Base.GetTeamBase(Team.TeamFlag.Player).ToggleAttackMode(); }


    public void AddGold() => GameController.AddPlayerGold_(100);
    public static void Restart_() => self.IsNotNull()?.Restart();
    public void Restart() => SceneManager.LoadScene(0);
}