using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Ultilities;


namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(HeroChoice))]
    public class PlayerController : GameCharacterController
    {
        #region Declarations
        //---Start of Declarations---
        #region Variables;
        [SerializeField] private GameInputSO gameInputSO;
        [SerializeField] private PlayerActionsSO playerActionsSO;
        [SerializeField] private PlayerStatSO playerStatSO;
        [SerializeField] private InventorySO playerInventorySO;
        [SerializeField] private Transform followPoint;
        public Transform FollowPoint => followPoint;
        public GameInputSO GameInputSO => gameInputSO;
        public PlayerStatSO PlayerStatSO => playerStatSO;
        public PlayerActionsSO PlayerActionsSO => playerActionsSO;
        public InventorySO PlayerInventorySO => playerInventorySO;

        public static PlayerController Instance;
        public PlayerAbilityManager PlayerAbilityManager { get; private set; }

        #endregion
        //--------
        #region Manage Player Movement
        public Vector3 MovementVector { get; private set; }

        private Vector3 directionVector;
        public override Vector3 DirectionVector
        {
            get
            {
                //if player is not performing any ability, use MovementVector from GameInput for direction
                if (StateMachine.CurrentState is not PlayerPerformAbilityState)
                {
                    directionVector = MovementVector;
                }
                return directionVector;
            }
            protected set { directionVector = value; }

        }
        /* => MovementVector; *///Inherited from GameCharacterController
        public PlayerMovement MovementCmp { get; private set; }
        public PlayerInventory InventoryCmp { get; private set; }

        #endregion
        //--------
        #region Manage Player's StateMachine && Bool ActionsState
        public PlayerStateMachine StateMachine { get; private set; }

        #endregion
        //--------
        #region Others
        public PlayerHealth HealthCmp { get; private set; }
        public PlayerCombat CombatCmp { get; private set; }
        public RaycastHit? HitInfo { get; private set; }
        private new CapsuleCollider collider;

        private int weaponLayer;
        private int enemyLayer;
        private int chestLayer;
        private int interactableLayermask;

        #endregion
        //---End of Declarations---
        #endregion

        #region Unity Methods
        //---Start of Unity Methods---
        private new void Awake()
        {
            base.Awake();
            //Set up Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            //Set up Player Components
            gameInputSO.Initialize();
            MovementCmp = GetComponent<PlayerMovement>();
            HealthCmp = GetComponent<PlayerHealth>();
            CombatCmp = GetComponent<PlayerCombat>();
            InventoryCmp = GetComponent<PlayerInventory>();
            //Set up Player's Layer
            weaponLayer = LayerMask.NameToLayer(GameConstants.WeaponLayer);
            enemyLayer = LayerMask.NameToLayer(GameConstants.EnemyLayer);
            chestLayer = LayerMask.NameToLayer(GameConstants.ChestLayer);
            interactableLayermask = (1 << enemyLayer) | (1 << chestLayer);
            //Other Setups
            collider = GetComponent<CapsuleCollider>();
        }
        private new void Start()
        {
            base.Start();
            PlayerAbilityManager = new PlayerAbilityManager();
            #region Subscribe to Events 
            gameInputSO.OnMovementVectorUpdated += GameInputSO_OnMovementVectorUpdated;
            gameInputSO.GameInput.Player.DefaultAttack.started += GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.HeavyAttack.started += GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.DashFrontAttack.started += GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.HealingSpell.started += GameInputSO_OnPlayerHealingAbilityStarted;
            gameInputSO.GameInput.Player.PickingUp.started += GameInputSO_OnPlayerPickingUpAbilityStarted;
            gameInputSO.GameInput.Player.Interact.started += GameInputSO_OnPlayerInteractStarted;
            GlobalEventManager.OnGivingPlayerBranchTree += GlobalEventManager_OnGivingPlayerBranchTree;

            #endregion
            StateMachine = new PlayerStateMachine(this);
            StateMachine.Initialize();
        }

        private void GlobalEventManager_OnGivingPlayerBranchTree(object sender, GameObject e)
        {
            PlayerAbilityManager.UnlockAbility(PlayerAbilityEnum.DefaultAttack);
        }

        private new void Update()
        {
            base.Update();
            StateMachine.Update();
            CheckingObjectForward();
        }
        private void OnDisable()
        {
            gameInputSO.CleanUp();
            gameInputSO.OnMovementVectorUpdated -= GameInputSO_OnMovementVectorUpdated;
            gameInputSO.GameInput.Player.DefaultAttack.started -= GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.HeavyAttack.started -= GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.DashFrontAttack.started -= GameInputSO_OnPlayerDealingDamageAbilityStarted;
            gameInputSO.GameInput.Player.HealingSpell.started -= GameInputSO_OnPlayerHealingAbilityStarted;
            gameInputSO.GameInput.Player.PickingUp.started -= GameInputSO_OnPlayerPickingUpAbilityStarted;
            gameInputSO.GameInput.Player.Interact.started -= GameInputSO_OnPlayerInteractStarted;
            GlobalEventManager.OnGivingPlayerBranchTree -= GlobalEventManager_OnGivingPlayerBranchTree;

        }
        //---End of Unity Methods---
        #endregion

        #region Inherited Methods
        //---Start of Inherited Methods---
        public override void InitializeStats()
        {
            //Health Stats
            HealthCmp.SetCurrentHealth(playerStatSO.defaultHealth);
            HealthCmp.SetMaxHealth(playerStatSO.maxHealth);
            //Combat Stats
            CombatCmp.SetAttackDamage(playerStatSO.defaultAttackDamage);
            //Movement Stats
            Agent.speed = playerStatSO.defaultSpeed;
        }

        public override void UpdateChildComponentParams()
        {
            CalculateSpeedBlendAnim();
        }

        //---End of Inherited Methods---
        #endregion

        #region Custom Methods
        //---Start of Custom Methods---

        public void MovePlayer()
        {
            if (gameInputSO.MovementVector == Vector3.zero) return;
            MovementCmp.MoveAgent();
        }
        private void CalculateSpeedBlendAnim()
        {
            //if player is not in PlayerIdleState or PlayerMoveState, return;
            if (StateMachine.CurrentState is not PlayerIdleState && StateMachine.CurrentState is not PlayerMoveState) { return; }
            //Decrease speedBlend when Player transit from PlayerMoveState to PlayerIdleState;
            else if (StateMachine.CurrentState is PlayerIdleState)
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
            //Increase speedBlend when Player transit from PlayerIdleState to PlayerMoveState;
            else if (StateMachine.CurrentState is PlayerMoveState)
            {
                speedBlend.value += speedBlend.GetAccelerateValue();
            }
            speedBlend.value = Mathf.Clamp01(speedBlend.value);
            playerActionsSO.OnPlayerMoveChangedRaised(this, new PlayerActionsSO.OnPlayerMovedArgs() { moveSpeedBlend = speedBlend.value });
        }
        public void ResetSpeedBlendAnim()
        {
            speedBlend.value = 0f;
            hasResetSpeedBlend = false;
        }
        public bool CheckingObjectForward()
        {
            float radius = collider.radius;
            float height = collider.height;
            bool result = false;
            Vector3 center1 = transform.position + collider.center + new Vector3(0, height * 0.5f - radius, 0);
            Vector3 center2 = transform.position + collider.center + new Vector3(0, radius - height * 0.5f, 0);
            result = Physics.CapsuleCast(center1, center2, radius / 2f, transform.forward, out RaycastHit hitInfo, playerStatSO.attackRange, interactableLayermask);
            if (result)
            {
                this.HitInfo = hitInfo;
            }
            else
            {
                this.HitInfo = null;
            }
            return result;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == weaponLayer)
            {
                InventoryCmp.AddToWeaponInRangeList(other.gameObject.GetComponent<Weapon>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == weaponLayer)
            {
                InventoryCmp.RemoveFromWeaponInRangeList(other.gameObject.GetComponent<Weapon>());
            }
        }




        #region Gizmo
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, playerStatSO.attackRange);

            CapsuleCollider acollider = GetComponent<CapsuleCollider>();
            float radius = acollider.radius;
            float height = acollider.height;
            Vector3 center1 = transform.position + acollider.center + Vector3.up * height * 0.5f - new Vector3(0, radius, 0);
            Vector3 center2 = transform.position + acollider.center - Vector3.up * height * 0.5f + new Vector3(0, radius, 0);

            Gizmos.color = Color.red;
            if (Physics.CapsuleCast(center1, center2, radius / 2f, transform.forward, out RaycastHit hitInfo, playerStatSO.attackRange))
            {
                Gizmos.DrawRay(transform.position + Vector3.up * height / 2f, transform.forward * hitInfo.distance);
                Gizmos.DrawWireSphere(hitInfo.point + Vector3.up * height * 0.5f - new Vector3(0, radius, 0), radius / 2);
                Gizmos.DrawWireSphere(hitInfo.point - Vector3.up * height * 0.5f + new Vector3(0, radius, 0), radius / 2);
            }
            else
            {
                Gizmos.DrawRay(transform.position + Vector3.up * height / 2f, transform.forward * playerStatSO.attackRange);
            }
        }
        #endregion
        //End of Custom Methods
        #endregion

        #region Event Listeners
        //---Start of Event Listeners---
        private void GameInputSO_OnPlayerInteractStarted(InputAction.CallbackContext obj)
        {
            if (!HitInfo.HasValue) return;
            GameObject interactObj = HitInfo.Value.transform.gameObject;
            if (interactObj.layer == chestLayer)
            {
                Chest chest = interactObj.GetComponent<Chest>();
                chest.OnPlayerInteract();
            }
        }


        private void GameInputSO_OnMovementVectorUpdated()
        {
            MovementVector = gameInputSO.MovementVector;
        }
        private void GameInputSO_OnPlayerDealingDamageAbilityStarted(InputAction.CallbackContext context)
        {
            if (!InventoryCmp.IsHoldingWeapon)
            {
                return;
            }
            HandlePlayerDealingDamageAbility();
            PlayerPerformAbility(context);
        }

        private void GameInputSO_OnPlayerHealingAbilityStarted(InputAction.CallbackContext context)
        {
            //if there is no potion, return but will add later
            if (!InventoryCmp.IsHoldingWeapon)
            {
                return;
            }
            PlayerPerformAbility(context);
        }

        private void GameInputSO_OnPlayerPickingUpAbilityStarted(InputAction.CallbackContext context)
        {
            PlayerPerformAbility(context);
        }

        private void PlayerPerformAbility(InputAction.CallbackContext context)
        {
            InputAction action = context.action;
            if (action == null) { return; }
            PlayerAbilityEnum actionEnum = gameInputSO.ActionsDict[action];
            var abilityToCheckUnlocked = PlayerAbilityManager.AbilityDict[actionEnum];
            if (!abilityToCheckUnlocked.isUnlocked) return;
            StateMachine.TransitionToState(PlayerStateEnum.PlayerPerformAbilityState);
            PlayerAbilityManager.PerformAbility(actionEnum);
        }
       
        private void HandlePlayerDealingDamageAbility()
        {
            CombatCmp.TargetEnemy = null;
            (bool isCorrect, CharacterHealth target) isEnemyForward = (false, null);
            if (HitInfo.HasValue)
            {
                isEnemyForward.isCorrect = HitInfo.Value.transform.TryGetComponent<CharacterHealth>(out CharacterHealth controller);
                isEnemyForward.target = controller;
            }
            if (isEnemyForward.isCorrect)
            {
                CombatCmp.TargetEnemy = isEnemyForward.target;
                directionVector = CombatCmp.TargetEnemy.transform.position - transform.position;
                directionVector.y = 0;
            }
            else
            {
                RotateTowardNearestEnemyInRange();
            }
        }

        public void RotateTowardNearestEnemyInRange()
        {
            if (CombatCmp.IsNeareastEnemyInRange(out CharacterHealth targetHealth))
            {
                directionVector = targetHealth.transform.position - transform.position;
                directionVector.y = 0;
                CombatCmp.TargetEnemy = targetHealth;
            };
        }

        public void SwitchGameInputActionMaps()
        {
            if (GameInputSO.GameInput.Player.enabled)
            {
                GameInputSO.GameInput.Player.Disable();
                GameInputSO.GameInput.UI.Enable();
            }
            else
            {
                GameInputSO.GameInput.UI.Disable();
                GameInputSO.GameInput.Player.Enable();
            }
        }





        //---End of Event Listeners---
        #endregion

    }
}

