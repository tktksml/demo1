using UnityEngine;


namespace FTK
{
    public interface ITeamable
    {
        public Team Team { get; set; }
    }
    [System.Serializable]
    public class Team
    {
        public static readonly Team PlayerTeam = new Team(TeamFlag.Player);
        public static readonly Team EnemyTeam = new Team(TeamFlag.Enemy);


        [System.Serializable]
        public enum TeamFlag
        {
            Player = 0,
            Enemy = 1
        }
        public Team(TeamFlag targetTeamFlag) { Flag = targetTeamFlag; }


        [field: SerializeField] public TeamFlag Flag { get; private set; } = TeamFlag.Player;








        public bool IsFriend(Team targetTeam) => !IsEnemy(targetTeam);
        public bool IsFriend(TeamFlag targetTeamFlag) => !IsEnemy(targetTeamFlag);


        public bool IsEnemy(Team targetTeam) => IsEnemy(targetTeam.Flag);
        public bool IsEnemy(TeamFlag targetTeamFlag) => Flag != targetTeamFlag;


        public TeamFlag GetMyEnemy() => Flag == TeamFlag.Player ? TeamFlag.Enemy : TeamFlag.Player;
    }
}