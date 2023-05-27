using UnityEngine;
using System.Collections;
using RPG.Ultilities;

namespace RPG.Character
{
    public class BossController : GameCharacterController
    {
        #region Declarations 

        [SerializeField] private BossStatSO bossStatSO;
        [SerializeField] private SummonEnemyPool enemyPool;
        [SerializeField] private ProjectilePool projectilePool;
        public SummonEnemyPool  EnemyPool => enemyPool;
        public ProjectilePool ProjectilePool => projectilePool;
        public BossStatSO BossStatSO => bossStatSO;
        public BossMovement MovementCmp { get; private set; }
        public BossHealth HealthCmp { get; private set; }
        public BossCombat CombatCmp { get; private set; }
        public BossStateMachine StateMachine { get; private set; }
        private Vector3 directionVector = Vector3.zero;

        public override Vector3 DirectionVector
        {
            get
            {
                if (StateMachine.CurrentState is BossChaseState ||
                    StateMachine.CurrentState is BossPerformAbilityState)
                {
                    directionVector = GetDirectionTowardsPlayer();
                }
                return directionVector;
            }
            protected set { directionVector = value; }
        }
        public BossAbilityManager BossAbilityManager { get; private set; }
        public struct BossActionsRecord
        {
            public bool isPerformingAbility;
            public bool isDefeated;
            public float speedBlendRestriction;
            public Vector3 OriginalPosition { get; set; }
            public Vector3 OriginalForwardDirection { get; set; }
            public void SetOriginalPosition(Vector3 args)
            {
                this.OriginalPosition = args;
            }
            public void SetOriginalForwardDirection(Vector3 args)
            {
                this.OriginalForwardDirection = args;
            }
        }
        public BossActionsRecord ActionsRecord;

        #endregion

        #region Events
        //public static event EventHandler<OnBossAppearedArgs> OnBossAppeared;
        //public class OnBossAppearedArgs : EventArgs
        //{
        //    public BossController BossController;
        //}







        #endregion

        #region Unity Methods
        private new void Awake()
        {
            base.Awake();
            MovementCmp = GetComponent<BossMovement>();
            HealthCmp = GetComponent<BossHealth>();
            CombatCmp = GetComponent<BossCombat>();

        }

        private new void Start()
        {
            base.Start();
            StateMachine = new BossStateMachine(this);
            BossAbilityManager = new BossAbilityManager(this);
            ActionsRecord = new BossActionsRecord() { isPerformingAbility = false, speedBlendRestriction = 1f };
            StateMachine.Initialize();
            StartCoroutine(RecordOriginalPosition());
        }
        private new void Update()
        {
            base.Update();
            StateMachine.Update();
            //test

        }

        #endregion

        #region Inherited Methods
        public override void InitializeStats()
        {
            //Health Stats
            HealthCmp.SetCurrentHealth(bossStatSO.defaultHealth);
            HealthCmp.SetOriginalHealth(bossStatSO.defaultHealth);
            ////Combat Stats
            CombatCmp.SetAttackDamage(bossStatSO.defaultAttackDamage);
            /////Movement Stats
            Agent.speed = bossStatSO.defaultSpeed;
        }

        public override void RotateAgent(Vector3 directionVector)
        {
            if (directionVector == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(directionVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * Agent.angularSpeed);
        }
        public override void SetDirectionVector(Vector3 newDirection)
        {
            directionVector = newDirection;
        }
        public override void UpdateChildComponentParams()
        {
            CalculateSpeedBlendAnim();
        }
        private void CalculateSpeedBlendAnim()
        {

            //if Boss is attacking or been defeated, return;
            if (StateMachine.CurrentState is BossPerformAbilityState || StateMachine.CurrentState is BossDefeatedState)
            {
                speedBlend.value = 0f;
                hasResetSpeedBlend = false;
            }
            //Decrease speedBlend when Boss transit from moving to BossIdleState;
            else if (StateMachine.CurrentState is BossIdleState)
            {
                //only reset speedblend once;
                if (hasResetSpeedBlend) return;
                if (speedBlend.value == 0f)
                {
                    //only reset speedblend once
                    hasResetSpeedBlend = true;
                    return;
                }
                else
                {
                    speedBlend.value -= speedBlend.GetDecelerateValue();
                }

            }
            //Increase speedBlend when Boss transit from BossIdleState to Moving;
            else
            {
                speedBlend.value += speedBlend.GetAccelerateValue();
            }
            //Boss can only walk in these two states;
            if (StateMachine.CurrentState is BossReturningState)
            {
                speedBlend.value = Mathf.Clamp(speedBlend.value, 0f, ActionsRecord.speedBlendRestriction);
            }
            else
            {
                speedBlend.value = Mathf.Clamp01(speedBlend.value);
            }
            BossEventManager.OnBossMoveChangedRaised(this, new BossEventManager.OnBossMoveChangedArgs() { moveSpeedBlend = speedBlend.value });
        }

        #endregion

        #region Custom Methods
        //---Movement and Rotate Section---
        //Begin--------------
        public Vector3 GetDirectionTowardsPlayer()
        {
            var playerPos = PlayerController.Instance.transform.position;
            var BossPos = transform.position;
            var direction = playerPos - BossPos;
            direction.y = 0;
            return direction;
        }
        public void ChasePlayer()
        {
            MovementCmp.MoveAgent(PlayerController.Instance.transform.position);
        }
        public float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        }
        //End----------------

        public void BossPerformAbility(BossAbilityEnum abilityEnum)
        {
            StateMachine.TransitionToState(BossStateEnum.BossPerformAbilityState);
            BossAbilityManager.PerformAbility(abilityEnum);

        }

        //---Combat Section---
        //Begin------------------

        private IEnumerator RecordOriginalPosition()
        {
            yield return new WaitForSeconds(0.3f);
            ActionsRecord.OriginalPosition = transform.position;
            ActionsRecord.OriginalForwardDirection = transform.forward;
        }

        #endregion

        #region Gizmo Drawing
        private void OnDrawGizmos()
        {
            //Default Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, bossStatSO.attackRange);
            //Chase Range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, bossStatSO.chaseRange);
            //Summoning Range
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, bossStatSO.summoningRange);
            //Spell Casting Range  
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, bossStatSO.spellCastingRange);
        }


        #endregion






    }

}
