
using UnityEngine;
using System;
using System.Collections;
using RPG.Ultilities;


namespace RPG.Character
{
    public class EnemyController : GameCharacterController

    {
        #region Declarations
        [SerializeField] private EnemyStatSO enemyStatSO;
        private EnemyActionsSO enemyActionsSO;
        public EnemyActionsSO EnemyActionsSO => enemyActionsSO;
        public EnemyStatSO EnemyStatSO => enemyStatSO;
        public EnemyMovement MovementCmp { get; private set; }
        public EnemyHealth HealthCmp { get; private set; }
        public EnemyCombat CombatCmp { get; private set; }
        public EnemyPatrol PatrolCmp { get; private set; }
        public EnemyStateMachine StateMachine { get; private set; }
        private Vector3 directionVector;
        public bool IsSummonedEnemy { get; set; } = false;

        public override Vector3 DirectionVector
        {
            get
            {
                if (StateMachine.CurrentState is EnemyChaseState ||
                    StateMachine.CurrentState is EnemyAttackState)
                {
                    directionVector = GetDirectionTowardsPlayer();
                }
                return directionVector;
            }
            protected set { directionVector = value; }
        }
        public struct EnemyActionsRecord
        {
            public bool isAttacking;
            public bool isDefeated;
            public bool isPatrolling;
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
        public EnemyActionsRecord ActionsRecord;

        #region Events
        public static event EventHandler<OnEnemyAppearedArgs> OnEnemyAppeared;
        public class OnEnemyAppearedArgs : EventArgs
        {
            public EnemyController enemyController;
        }
        #endregion


        #endregion

        #region Unity Methods
        private new void Awake()
        {
            base.Awake();
            MovementCmp = GetComponent<EnemyMovement>();
            HealthCmp = GetComponent<EnemyHealth>();
            CombatCmp = GetComponent<EnemyCombat>();
            PatrolCmp = GetComponentInChildren<EnemyPatrol>();
            enemyActionsSO = ScriptableObject.CreateInstance<EnemyActionsSO>();
        }
        private void OnEnable()
        {
            if (!IsSummonedEnemy) return;
            InitializeStats();
            gameObject.layer = LayerMask.NameToLayer(GameConstants.EnemyLayer);
            ActionsRecord = new EnemyActionsRecord() { isAttacking = false, isPatrolling = false, speedBlendRestriction = 0.7f };
            if (StateMachine.CurrentState != null)
            {
                StateMachine.Initialize();
            }
            StartCoroutine(RecordOriginalPosition());
        }

        private new void Start()
        {
            base.Start();
            StateMachine = new EnemyStateMachine(this);
            ActionsRecord = new EnemyActionsRecord() { isAttacking = false, isPatrolling = false, speedBlendRestriction = 0.7f };
            StateMachine.Initialize();
            StartCoroutine(RecordOriginalPosition());
        }
        private new void Update()
        {
            base.Update();
            StateMachine.Update();
            //Debug.Log("Is Cloned: " + IsSummonedEnemy);
        }


        #endregion

        #region Inherited Methods
        public override void InitializeStats()
        {
            //Health Stats
            HealthCmp.SetCurrentHealth(enemyStatSO.defaultHealth);
            HealthCmp.SetOriginalHealth(enemyStatSO.defaultHealth);
            ////Combat Stats
            CombatCmp.SetAttackDamage(enemyStatSO.defaultAttackDamage);
            /////Movement Stats
            Agent.speed = enemyStatSO.defaultSpeed;
            //////Notify PlayerCombat.cs when Enemy is appeared
            OnEnemyAppeared?.Invoke(this, new OnEnemyAppearedArgs { enemyController = this });
        }

        public override void RotateAgent(Vector3 directionVector)
        {
            //if Enemy is in EnemyPatrolState, don't rotate as default, but rely on SplineAnimate to automatically Rotate;
            if (StateMachine.CurrentState is EnemyPatrolState) return;
            if (directionVector == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(directionVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        public override void SetDirectionVector(Vector3 newDirection)
        {
            directionVector = newDirection;
        }
        public override void UpdateChildComponentParams()
        {
            //Update Anim Params for EnemyVisual.cs
            CalculateSpeedBlendAnim();
        }
        private void CalculateSpeedBlendAnim()
        {
            //if Enemy is attacking or been defeated, return;
            if (StateMachine.CurrentState is EnemyAttackState || StateMachine.CurrentState is EnemyDefeatedState) { return; }
            //Decrease speedBlend when Enemy transit from moving to EnemyIdleState;
            else if (StateMachine.CurrentState is EnemyIdleState)
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
            //Increase speedBlend when Enemy transit from EnemyIdleState to Moving;
            else
            {
                speedBlend.value += speedBlend.GetAccelerateValue();
            }
            //Restric speedBlend value to speedBlendRestriction(0.7f) when Enemy is in EnemyPatrolState or EnemyReturningState;
            //enemy can only walk in these two states;
            if (StateMachine.CurrentState is EnemyPatrolState || StateMachine.CurrentState is EnemyReturningState)
            {
                speedBlend.value = Mathf.Clamp(speedBlend.value, 0f, ActionsRecord.speedBlendRestriction);
            }
            else
            {
                speedBlend.value = Mathf.Clamp01(speedBlend.value);
            }
            EnemyActionsSO.OnEnemyMovedChangedRaised(this, new EnemyActionsSO.OnEnemyMoveChangedArgs() { moveSpeedBlend = speedBlend.value });
        }


        public override IEnumerator HandleDefeatedAfterSec(float sec)
        {
            yield return new WaitForSeconds(sec);
            if(IsSummonedEnemy)
            {
                BossEventManager.OnReturnSummonedEnemyToPoolRaised(
                    null, new BossEventManager.OnReturnSummonedEnemyToPoolArgs() { summonedEnemy = this.gameObject });
            }
            else
            {
                Destroy(gameObject);
            }
        }









        #endregion

        #region Custom Methods
        //---Movement and Rotate Section---
        //Begin--------------
        public Vector3 GetDirectionTowardsPlayer()
        {
            var playerPos = PlayerController.Instance.transform.position;
            var enemyPos = transform.position;
            var direction = playerPos - enemyPos;
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



        //---Combat Section---
        //Begin------------------
        public void AttackPlayer()
        {
            CombatCmp.DefaultAttack();
            ActionsRecord.isAttacking = true;
        }

        public bool HasPatrolComponent()
        {
            return PatrolCmp != null;
        }

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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, EnemyStatSO.attackRange);
            //------------
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, EnemyStatSO.chaseRange);
        }
        #endregion

    }
}





