using KyokoLib.ServiceLib;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


namespace FTK.GamePlayLib.MapLib.BuildingsLib
{
    public class Base : MonoBehaviour, IInitable, ITeamable, IHealthable
    {
        private static event Action<Base, bool> onAttackModeToggled = null;
        public static event Action<Base, bool> OnAttackModeToggled
        {
            add
            {
                if (onAttackModeToggled == null || !onAttackModeToggled.GetInvocationList().Contains(value))
                    onAttackModeToggled += value;
            }
            remove
            {
                if (onAttackModeToggled != null && onAttackModeToggled.GetInvocationList().Contains(value))
                    onAttackModeToggled -= value;
            }
        }


        private static List<Base> Bases = new List<Base>();


        [Header("Settings")]
        [field: SerializeField] public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        [field: SerializeField] public Team Team { get; set; } = new Team(Team.TeamFlag.Player);


        public bool IsAttackMode { get; private set; } = false;


        private Transform transformCache = null;
        public Transform TransformCache
        {
            get
            {
                if (transformCache == null)
                    transformCache = transform;
                return transformCache;
            }
        }








        public void OnHealthDecreased() { }
        public void OnHealthBelowZero() { Test.Restart_(); }
        public Vector3 GetPosition() => TransformCache.position;


        public void CoreInit()
        {
            CurrentHealth = MaxHealth;


            Bases.Add(this);
        }
        public void PostInit()
        {
            //Создаем пушки на башни
            // CoroutineHelper.DelayFrames(() => { UnitController.TryCreateUnit_<Tower>(Team, out _); }, 1);
        }
        private void OnDestroy() { Bases.Remove(this); }


        public static Base GetTeamBase(Team targetTeam) => Bases.Find(_base => _base.Team.IsFriend(targetTeam));
        public static Base GetTeamBase(Team.TeamFlag teamFlag) => Bases.Find(_base => _base.Team.IsFriend(teamFlag));


        public void ToggleAttackMode()
        {
            IsAttackMode = !IsAttackMode;
            onAttackModeToggled?.Invoke(this, IsAttackMode);
        }
    }
}