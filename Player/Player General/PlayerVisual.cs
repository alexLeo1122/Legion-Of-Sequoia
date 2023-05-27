using System;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class PlayerVisual : MonoBehaviour
    {
        #region Declarations
        [SerializeField] private Transform rightHandSlot;
        [SerializeField] private Transform leftHandSlot;
        private PlayerController playerController;
        private PlayerActionsSO playerActionsSO;
        private Animator animatorCmp;
        private int moveSpeedBlendHash;
        private int defaultAttackTriggerAnimHash;
        private int heavyAttackTriggerAnimHash;
        private int dashFrontAttackTriggerAnimHash;
        private int healingSpellTriggerAnimHash;
        private int pickingUpTriggerAnimHash;
        private int isDefeatedAnimHash;
        #endregion

        #region Unity Methods

        private void Start()
        {
            playerController = GetComponentInParent<PlayerController>();
            playerController.InventoryCmp.RightHandSlot = rightHandSlot;
            playerController.InventoryCmp.LeftHandSlot = leftHandSlot;
            animatorCmp = GetComponent<Animator>();
            moveSpeedBlendHash = Helpers.StringToHash(GameConstants.MoveSpeedBlend);
            defaultAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.DefaultAttackTriggerAnim);
            heavyAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.HeavyAttackTriggerAnim);
            dashFrontAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.DashFrontAttackTriggerAnim);
            healingSpellTriggerAnimHash = Helpers.StringToHash(GameConstants.HealingSpellTriggerAnim);
            pickingUpTriggerAnimHash = Helpers.StringToHash(GameConstants.PickingUpTriggerAnim);
            isDefeatedAnimHash = Helpers.StringToHash(GameConstants.IsDefeatedBoolAnim);
            //sub to playerActionsSO
            if (playerActionsSO != null) return;
            playerActionsSO = playerController.PlayerActionsSO;
            playerActionsSO.OnPlayerMoved += PlayerActionsSO_OnPlayerMoved;
            playerActionsSO.OnPlayerDefaultAttack += PlayerActionsSO_OnPlayerDefaultAttack;
            playerActionsSO.OnPlayerHeavyAttack += PlayerActionsSO_OnPlayerHeavyAttack;
            playerActionsSO.OnPlayerDashFrontAttack += PlayerActionsSO_OnPlayerDashFrontAttack;
            playerActionsSO.OnPlayerHealingSpell += PlayerActionsSO_OnPlayerHealingSpell;
            playerActionsSO.OnPlayerPickingUp += PlayerActionsSO_OnPlayerPickingUp;
            playerActionsSO.OnPlayerDefeated += PlayerActionsSO_OnPlayerDefeated;
        }
        private void OnDisable()
        {
            playerActionsSO.OnPlayerMoved -= PlayerActionsSO_OnPlayerMoved;
            playerActionsSO.OnPlayerDefaultAttack -= PlayerActionsSO_OnPlayerDefaultAttack;
            playerActionsSO.OnPlayerHeavyAttack -= PlayerActionsSO_OnPlayerHeavyAttack;
            playerActionsSO.OnPlayerDashFrontAttack -= PlayerActionsSO_OnPlayerDashFrontAttack;
            playerActionsSO.OnPlayerHealingSpell -= PlayerActionsSO_OnPlayerHealingSpell;
            playerActionsSO.OnPlayerPickingUp -= PlayerActionsSO_OnPlayerPickingUp;
            playerActionsSO.OnPlayerDefeated -= PlayerActionsSO_OnPlayerDefeated;
        }

        #endregion

        #region Event Listeners
        private void PlayerActionsSO_OnPlayerDefaultAttack(object sender, PlayerActionsSO.OnPlayerDefaultAttackArgs e)
        {
            animatorCmp.SetTrigger(defaultAttackTriggerAnimHash);
        }
        private void PlayerActionsSO_OnPlayerHeavyAttack(object sender, PlayerActionsSO.OnPlayerHeavyAttackArgs e)
        {
            animatorCmp.SetTrigger(heavyAttackTriggerAnimHash);
        }

        private void PlayerActionsSO_OnPlayerDashFrontAttack(object sender, PlayerActionsSO.OnPlayerDashFrontAttackArgs e)
        {
            animatorCmp.SetTrigger(dashFrontAttackTriggerAnimHash);
        }


        private void PlayerActionsSO_OnPlayerHealingSpell(object sender, PlayerActionsSO.OnPlayerHealingSpellArgs e)
        {
            animatorCmp.SetTrigger(healingSpellTriggerAnimHash);
        }

        private void PlayerActionsSO_OnPlayerPickingUp(object sender, PlayerActionsSO.OnPlayerPickingUpArgs e)
        {
            if (playerController.InventoryCmp.CurrentWeapon != null)
            {
                TransitionToIdleState();
                return;
            }
            animatorCmp.SetTrigger(pickingUpTriggerAnimHash);
        }


        private void PlayerActionsSO_OnPlayerMoved(object sender, PlayerActionsSO.OnPlayerMovedArgs e)
        {
            animatorCmp.SetFloat(moveSpeedBlendHash, e.moveSpeedBlend);
        }
        private void PlayerActionsSO_OnPlayerIdle(object sender, EventArgs e)
        {
            animatorCmp.SetFloat(moveSpeedBlendHash, 0f);
        }
        private void PlayerActionsSO_OnPlayerDefeated(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(isDefeatedAnimHash);
        }
        #endregion

        #region Animation Event
        //Default Attack
        private void OnDefaultAttackLanded()
        {
            DealingDamage();
        }
        private void OnDefaultAttackEnd()
        {
            TransitionToIdleState();
        }
        //Heavy Attack
        private void OnHeavyAttackLanded()
        {
            DealingDamage();
        }
        private void OnHeavyAttackEnd()
        {
            TransitionToIdleState();
        }

        private void OnDashFrontAttackLanded()
        {
            DealingDamage();
        }

        private void OnDashFrontAttackEnd()
        {
            TransitionToIdleState();
        }

        private void OnHealingSpellAffect()
        {
            CastingSpellAffect();
            GlobalEventManager.OnHealingTakeEffectRaised();
        }
        private void OnHealingSpellEnd()
        {
            TransitionToIdleState();

        }

        private void OnPickingUpAffect()
        {
            playerController.InventoryCmp.EquipItem();
        }

        private void OnPickingUpEnd()
        {
            TransitionToIdleState();
        }



        private void OnDefeatedAnimEnd()
        {
            GlobalEventManager.OnGameEndRaised(false);
        }


        public void DealingDamage()
        {
            playerController.CombatCmp.HandleAbilityDealDamage();
        }

        public void CastingSpellAffect()
        {
            playerController.CombatCmp.HandleCastingSpellAffectTarget();
        }

        public void TransitionToIdleState()
        {
            playerController.StateMachine.TransitionToState(PlayerStateEnum.PlayerIdleState);
        }

        #endregion
    }

}
