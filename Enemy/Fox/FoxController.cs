using UnityEngine;
using RPG.Character;
using RPG.Ultilities;
using System.Collections;

namespace RPG.Quest
{
    public class FoxController : GameCharacterController
    {
        #region Declarations
        [SerializeField] private FoxVisual foxVisual;
        [SerializeField] private EnemyAttackRange attackRange;
        [SerializeField] private Transform foxCave;
        public FoxVisual FoxVisual => foxVisual;
        public float Damage { get; private set; } = 3f;
        private float timeToWait = 0.5f;
        private CharacterHealth player => PlayerController.Instance.HealthCmp;
        public bool IsAttacking { get; set; }
        public bool IsPlayerInChaseRange { get ; set; } 
        public bool IsPlayerInAttackRange { get; set; }
        public bool isWaitToChase { get; set; } = false;
        public bool IsWaitBeforeRun { get; set; } = false;
        public bool IsFinishWaiting { get; set; } = false;
        public bool IsRunBackToCave { get; set; } = false;
        public bool IsDefeated { get; set; } = false;
        public bool IsDefeatedAnimEnd { get; set; } = false;

        public FoxStateMachine StateMachine { get; private set; }
        #endregion

        #region Unity Methods
        public override  void Start()
        {
            StateMachine = new FoxStateMachine(this);
            StateMachine.Initialize();
            //Agent = GetComponent<NavMeshAgent>();
            GlobalEventManager.OnPlayerEnterAttackRange += GlobalEventManager_OnPlayerEnterAttackRange;
            GlobalEventManager.OnPlayerExitAttackRange += GlobalEventManager_OnPlayerExitAttackRange;
        }
        public override void Update()
        {
           //for fututre extension
           StateMachine.Update();   
        }
        private void OnDisable()
        {
            GlobalEventManager.OnPlayerEnterAttackRange -= GlobalEventManager_OnPlayerEnterAttackRange;
            GlobalEventManager.OnPlayerExitAttackRange -= GlobalEventManager_OnPlayerExitAttackRange;
        }
        #endregion

        #region Event Handlers

        private void GlobalEventManager_OnPlayerExitAttackRange(object sender, GlobalEventManager.OnPlayerExitAttackRangeArgs e)
        {
            if (StateMachine.CurrentState is FoxDefeatedState) return;
            StopAgent();
            StateMachine.TransitionToState(FoxStateEnum.FoxIdleState);
        }

        private void GlobalEventManager_OnPlayerEnterAttackRange(object sender, GlobalEventManager.OnPlayerEnterAttackRangeArgs e)
        {
            if (StateMachine.CurrentState is FoxDefeatedState) return;
            StateMachine.TransitionToState(FoxStateEnum.FoxAttackState);
        }
        #endregion

        #region Trigger Methods
        private void OnTriggerEnter(Collider other)
        {
            if (StateMachine.CurrentState is FoxDefeatedState) return;
            if (other.gameObject.CompareTag("Player"))
            {
                StateMachine.TransitionToState(FoxStateEnum.FoxChasingState);
                IsPlayerInChaseRange = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (StateMachine.CurrentState is FoxDefeatedState) return;
            if (other.gameObject.CompareTag("Player"))
            {
                StopAgent();
                StateMachine.TransitionToState(FoxStateEnum.FoxIdleState);
                IsPlayerInChaseRange = false;
            }
        }
        #endregion

        #region Custom Methods
        public void StopAgent()
        {
            Agent.velocity = Vector3.zero;
            Agent.ResetPath();
        }
        public void ChasePlayer()
        {
            Agent.SetDestination(player.transform.position);
        }
        public void RotateTowardTarget(Vector3 directionVector)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRoation = Quaternion.LookRotation(directionVector);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRoation, Agent.angularSpeed * Time.deltaTime);
        }
        public void AttackPlayer()
        {
            Vector3 directionVector = PlayerController.Instance.transform.position - transform.position;
            RotateTowardTarget(directionVector);
        }
        public IEnumerator WaitAndRun(float time)
        {
            yield return new WaitForSeconds(time);
            IsWaitBeforeRun = false;
            IsFinishWaiting = true;
        }
        public void EnemyWaitAndRun()
        {
            if (IsFinishWaiting) return;
            IsWaitBeforeRun = true;
            StartCoroutine(WaitAndRun(timeToWait));
        }
        public void RunbackToCave()
        {
            if (!IsRunBackToCave) {
                foxVisual.HandleFoxRunAnim();
                IsRunBackToCave = true;
            }
            Vector3 directionVector = foxCave.transform.position - transform.position;
            RotateTowardTarget(directionVector);
            Agent.SetDestination(foxCave.position);
        }
        #endregion

        #region Inherited Methods
        public override void InitializeStats()
        {
            //throw new NotImplementedException();
        }

        public override void UpdateChildComponentParams()
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}


