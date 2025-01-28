using KyokoLib.ServiceLib;
using FTK.GamePlayLib.MapLib.BuildingsLib;
using FTK.GamePlayLib.UnitLib.AttackLib;
using FTK.GamePlayLib.UnitLib.VisualLib;
using System.Linq;
using System;
using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace FTK.GamePlayLib.UnitLib
{
    [SelectionBase]
    public abstract class Unit : MonoBehaviour, IInitable, IHealthable, ITeamable
    {
        private static event Action<Unit> onUnitedDied = null;
        public static event Action<Unit> OnUnitDied
        {
            add
            {
                if (onUnitedDied == null || !onUnitedDied.GetInvocationList().Contains(value))
                    onUnitedDied += value;
            }
            remove
            {
                if (onUnitedDied != null && onUnitedDied.GetInvocationList().Contains(value))
                    onUnitedDied -= value;
            }
        }


        private static event Action<Unit> onUnitDamaged = null;
        public static event Action<Unit> OnUnitDamaged
        {
            add
            {
                if (onUnitDamaged == null || !onUnitDamaged.GetInvocationList().Contains(value))
                    onUnitDamaged += value;
            }
            remove
            {
                if (onUnitDamaged != null && onUnitDamaged.GetInvocationList().Contains(value))
                    onUnitDamaged -= value;
            }
        }


        private static event Action<Unit> onUnitedSpawned = null;
        public static event Action<Unit> OnUnitSpawned
        {
            add
            {
                if (onUnitedSpawned == null || !onUnitedSpawned.GetInvocationList().Contains(value))
                    onUnitedSpawned += value;
            }
            remove
            {
                if (onUnitedSpawned != null && onUnitedSpawned.GetInvocationList().Contains(value))
                    onUnitedSpawned -= value;
            }
        }


        [Header("Settings")]
        [field: SerializeField] public int MaxHealth { get; set; } = 100;
        [field: SerializeField] public float MoveSpeed { get; set; } = 8;
        [field: SerializeField] public int AttackDamage { get; set; } = 10;
        [field: SerializeField] public int AttackSpeed { get; set; } = 200;
        [field: SerializeField] public float AttackRange { get; set; } = 1.5f;
        [field: SerializeField] public float AgroRange { get; set; } = 5f;
        [field: SerializeField] public Team Team { get; set; }
        [field: SerializeField] public Liner Liner { get; set; } = Liner.FrontLiner;


        [Header("Visual Require Components")]
        [SerializeField] private UnitVisual playerTeamVisual = null;
        [SerializeField] private UnitVisual enemyTeamVisual = null;
        public UnitVisual CurrentTeamVisual { get; private set; } = null;


        [Header("Require Components")]
        [field: SerializeField] public NavMeshAgent UnitNavMesh { get; private set; } = null;
        [field: SerializeField] public Visor AgroVisor { get; private set; } = null;
        [field: SerializeField] public Visor AttackVisor { get; private set; } = null;


        public Job CurrentJob { get; protected set; } = null;
        public AttackModel AttackModel { get; private set; }
        public int CurrentHealth { get; set; }


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


        private Base currentBaseCache = null;


        private bool isInited = false;








        public abstract bool CanBeCreated(Team team);
        public void Init(Team targetTeam)
        {
            CurrentHealth = MaxHealth;


            //Определяем к какой команде относится юнит
            Team = targetTeam;


            //Включает конкретный визуал команды
            if (Team.IsFriend(Team.TeamFlag.Player))
            {
                playerTeamVisual.Toggle(true);
                enemyTeamVisual.Toggle(false);
                CurrentTeamVisual = playerTeamVisual;
            }
            else
            {
                playerTeamVisual.Toggle(false);
                enemyTeamVisual.Toggle(true);
                CurrentTeamVisual = enemyTeamVisual;
            }


            //Запускаем все эвенты
            Services.AddService(this);


            //Инитим модель атаки
            AttackModel = InitAttackModel();


            AttackVisor.IsNotNull()?.UpdateRadius(AttackModel.BaseSettings.AttackRange);
            AgroVisor.IsNotNull()?.UpdateRadius(AttackModel.BaseSettings.AgroRange);


            isInited = true;
        }


        private void Start()
        {
            if (isInited == false)
                Init(Team);
        }
        public virtual void PreInit() { UnitNavMesh.speed = MoveSpeed; }
        public virtual void PostInit()
        {
            onUnitedSpawned?.Invoke(this);
            FindJob();
        }
        public virtual void SubscribeEvent() { }
        public virtual void UnSubscribeEvent() { }


        public virtual void Update() { CurrentJob?.OnUpdate(); }
        public virtual void FixedUpdate() { CurrentJob?.OnFixedUpdate(); }
        public virtual void OnDestroy()
        {
            CurrentJob?.OnDestroy();
            UnSubscribeEvent();
        }


        public virtual void FindJob()
        {
            //Проверяем есть ли работа в зоне действия
            AgroVisor.IsNotNull()?.ReTrigger();
            AttackVisor.IsNotNull()?.ReTrigger();
        }


        public void SetJob(Job newJob)
        {
            if (CurrentJob != null)
            {
                //Некоторые задачи имеют приоритет, проверяем может ли новая задача принудительно завершить старую
                if (CurrentJob.IsJobInteractable(newJob) == false)
                    return;
                //Отменяем старую задачу
                CurrentJob.OnJobInteracted(newJob);
            }


            //Обновляем ссылку
            CurrentJob = newJob;
            //Инитим работу
            CurrentJob.Init(this);
        }
        public void SetDestination(Vector3 targetDestination)
        {
            if (UnitNavMesh.isOnNavMesh)
            {
                if (UnitNavMesh.destination != targetDestination)
                {
                    CurrentTeamVisual.OnStartRun();
                }
                UnitNavMesh.SetDestination(targetDestination);
                UnitNavMesh.isStopped = false;
            }
        }
        public void StopMoving()
        {
            if (UnitNavMesh.isOnNavMesh)
            {
                //Как толкьо юнит остановился - запускаем анимацию айдла
                if (UnitNavMesh.isStopped == false)
                    CurrentTeamVisual.OnIdle();
                UnitNavMesh.isStopped = true;
            }
        }


        public abstract AttackModel InitAttackModel();
        public void OnAttack()
        {
            //Запускаем визуал атаки
            CurrentTeamVisual.OnAttack();
        }


        public void OnHealthDecreased()
        {
            //Запускаем визуал того, что юнит был атакован
            CurrentTeamVisual.OnAttacked();


            onUnitDamaged?.Invoke(this);
        }
        public void OnHealthBelowZero()
        {
            //Просто умираем
            Dead();
        }
        private void Dead()
        {
            //Уничтожаем коллайдер чтобы метрое тело никому не мешало
            Utils.Destroy(gameObject.GetComponent<Collider>());
            //Запускаем анимацию смерти
            ActivateDeadAnimation();
            //Уничтожаем скрипт чтобы не мешался
            Utils.Destroy(this);
        }
        private void ActivateDeadAnimation()
        {
            Destroy(gameObject, 1);
            TransformCache.DOScale(Vector3.zero, 1);
        }


        public virtual Vector3 GetPosition() => TransformCache.position;


        /// <summary>
        /// Вовзращает базу юнита
        /// </summary>
        /// <returns></returns>
        public Base GetUnitBase()
        {
            if (currentBaseCache == null)
                currentBaseCache = Base.GetTeamBase(Team);
            return currentBaseCache;
        }


        #if UNITY_EDITOR
        private void OnDrawGizmos() { Handles.Label(TransformCache.position, $"{CurrentHealth}/{MaxHealth}"); }
        #endif
    }
}