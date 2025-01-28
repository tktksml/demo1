using FTK.GamePlayLib.UnitLib;
using KyokoLib.ExtensionLib;
using UnityEngine;


namespace FTK.GamePlayLib.MapLib.BuildingsLib
{
    public class ClaimableTower : MonoBehaviour, IClaimable
    {
        [Header("Require Components")]
        [SerializeField] private GameObject neutralVisual = null;
        [SerializeField] private GameObject playerVisual = null;
        [SerializeField] private GameObject enemyVisual = null;
        [SerializeField] private Transform unitSpawnPoint = null;


        [Header("Settings")]
        [SerializeField] private float claimTime = 3;
        [SerializeField] private float spawnTime = 3;


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


        private Team currentClaimingTeam = new Team(Team.TeamFlag.Player);
        private float currentClaimingValue = 0;


        private float currentSpawnTimer = 0;


        public bool IsFullyClaimed { get; set; } = false;
        public Team ClaimedTeam { get; set; } = null;








        private void Start() => UpdateSpawnTimer();
        private void Update()
        {
            if (IsFullyClaimed)
            {
                currentSpawnTimer -= Time.deltaTime;
                if (currentSpawnTimer < 0)
                {
                    UpdateSpawnTimer();


                    if (UnitController.TryCreateUnit_<Warrior>(ClaimedTeam, out var spawnedUnit))
                        spawnedUnit.TransformCache.position = unitSpawnPoint.position;
                }
            }
        }
        public void OnClaiming(float claimValue, Team claimTeam)
        {
            //Если текущая команда итак захватывает строение, то нужно добавить прогресс
            if (currentClaimingTeam.IsFriend(claimTeam))
                currentClaimingValue += claimValue;
            else
                currentClaimingValue -= claimValue;


            //Проверяем на смену лидера захвата
            if (currentClaimingValue < 0)
            {
                currentClaimingTeam = claimTeam;
                currentClaimingValue = 0;
            }


            //Проверяем на полный захват
            if (currentClaimingValue >= claimTime)
                OnFullyClaimed(claimTeam);
        }
        private void OnFullyClaimed(Team claimedTeam)
        {
            //Записываем что здание было полностью захвачено
            IsFullyClaimed = true;
            //Записываем кто именно захватил
            ClaimedTeam = claimedTeam;


            neutralVisual.Toggle(false);
            if (claimedTeam.IsFriend(Team.TeamFlag.Player))
            {
                playerVisual.Toggle(true);
                enemyVisual.Toggle(false);
            }
            else
            {
                playerVisual.Toggle(false);
                enemyVisual.Toggle(true);
            }
        }


        private void UpdateSpawnTimer() => currentSpawnTimer = spawnTime;
    }
}