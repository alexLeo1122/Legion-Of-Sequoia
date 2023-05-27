using UnityEngine;
using RPG.Character;
using UnityEngine.AI;
using RPG.Ultilities;
using System.Collections;

namespace RPG.Allies
{
    public class SlimeController : MonoBehaviour
    {
        #region Declarations
        private PlayerController playerController => PlayerController.Instance;
        public SlimeStateMachine StateMachine { get; private set; }
        public NavMeshAgent Agent { get; set; }
        private bool isPlayerInRange = true;
        public bool IsPlayerInRange => isPlayerInRange;
        private float elaspedTime = 0f;
        private float followTimeMax = 1.5f;
        private float followSpeedMultiply = 3f;
        private float defaultSpeedMultiply = 0.5f;
        private float defaultDamageMultiply = 0.2f;
        public float DefaultDamage => playerController.PlayerStatSO.defaultAttackDamage * defaultDamageMultiply;
        public struct SlimeStatusMonitor
        {
            public bool isPerformAbility;
        }
        public SlimeStatusMonitor statusMonitor;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        private void OnEnable()
        {
            //DefaultDamage = playerController.PlayerStatSO.defaultAttackDamage * defaultDamageMultiply;
            GlobalEventManager.OnAllyFollowPlayerMove += GlobalEventManager_OnFollowPlayerMove;
            statusMonitor = new SlimeStatusMonitor() { isPerformAbility = false };
            StateMachine = new SlimeStateMachine(this);
            StateMachine.Initialize();
        }
        private void OnDisable()
        {
            GlobalEventManager.OnAllyFollowPlayerMove -= GlobalEventManager_OnFollowPlayerMove;
        }

        private void Update()
        {
            StateMachine.Update();
            //Debug.Log($"Slime Current state is: {StateMachine.CurrentState}, isperform: {statusMonitor.isPerformAbility}");
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GameConstants.PlayerTag))
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // if player moves too far, reposition Slime
            if (other.gameObject.CompareTag(GameConstants.PlayerTag))
            {
                isPlayerInRange = false;
                StartCoroutine(RepositionToFollowPoint());
            }
        }


        #endregion

        #region Event Handlers
        private void GlobalEventManager_OnFollowPlayerMove(object sender, GlobalEventManager.OnFollowPlayerMoveArgs e)
        {
            if (!isPlayerInRange) return;
            Agent.Move(e.movementVector * Time.deltaTime * e.playerSpeed * defaultSpeedMultiply);
        }

        private IEnumerator RepositionToFollowPoint()
        {
            elaspedTime = 0f;
            Agent.speed = playerController.Agent.speed * followSpeedMultiply;
            while (elaspedTime < followTimeMax)
            {
                Agent.SetDestination(playerController.FollowPoint.position);
                yield return null;
            }
            transform.position = playerController.FollowPoint.position;
        }
        #endregion

        #region Custom Methods
        public void StopAgent()
        {
            Agent.velocity = Vector3.zero;
            Agent.ResetPath();
        }

        public void PerformAbility()
        {
            if (!isPlayerInRange) return;
            var target = playerController.CombatCmp.TargetEnemy;
            if (target == null) return;
            RotateTowardTarget(target.transform.position);
        }

        public void RotateTowardTarget(Vector3 target)
        {
            var directionVector = target - transform.position;
            transform.forward = directionVector;
        }
        #endregion
    }
}


