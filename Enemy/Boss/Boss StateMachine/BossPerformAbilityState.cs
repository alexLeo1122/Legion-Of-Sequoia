using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public class BossPerformAbilityState : BossBaseState
    {
        private Vector3 originalPosition;
        public BossPerformAbilityState(BossController controller) : base(controller) { }
        public override void Enter()
        {
            bossController.MovementCmp.StopAgent();
            originalPosition = bossController.ActionsRecord.OriginalPosition;
        }
        public override void Exit()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
        }
        public override void Update()
        {
            if (PlayerController.Instance.StateMachine.CurrentState is PlayerDefeatedState)
            {
                bossController.StateMachine.TransitionToState(BossStateEnum.BossIdleState);
            }

            //If player leave Boss Battle Area while Boss is performing ability
            if (DistanceToPlayer > bossController.BossStatSO.spellCastingRange)
            {
                bossController.Agent.SetDestination(originalPosition);
            }
            else if (DistanceToPlayer <= bossController.BossStatSO.spellCastingRange &&
                    DistanceToPlayer > bossController.BossStatSO.summoningRange)
            {
                if (bossController.ActionsRecord.isPerformingAbility) return;
                if (bossController.CombatCmp.IsCastFireballCoolDown) return;
                bossController.ActionsRecord.isPerformingAbility = true;
                bossController.BossAbilityManager.PerformAbility(BossAbilityEnum.FireBallSpell);
            }
            else if (DistanceToPlayer <= bossController.BossStatSO.summoningRange
                    && DistanceToPlayer > bossController.BossStatSO.chaseRange)
            {
                if (bossController.ActionsRecord.isPerformingAbility) return;
                if (bossController.CombatCmp.IsSummonEnemyCoolDown) return;
                if (bossController.CombatCmp.IsSummonEnemiesReachMax) return;
                bossController.ActionsRecord.isPerformingAbility = true;
                bossController.BossAbilityManager.PerformAbility(BossAbilityEnum.SummoningSpell);
            }
            else if (DistanceToPlayer <= bossController.BossStatSO.attackRange)
            {
                if (bossController.ActionsRecord.isPerformingAbility) return;
                bossController.ActionsRecord.isPerformingAbility = true;
                bossController.BossAbilityManager.PerformAbility(BossAbilityEnum.DefaultAttack);
            }
            else if (DistanceToPlayer <= bossController.BossStatSO.chaseRange
                    && DistanceToPlayer > bossController.BossStatSO.attackRange)
            {
                if (bossController.ActionsRecord.isPerformingAbility) return;
                if (bossController.CombatCmp.IsCastFireballCoolDown) return;
                bossController.ActionsRecord.isPerformingAbility = true;
                bossController.BossAbilityManager.PerformAbility(BossAbilityEnum.FireBallSpell);
            }
        }

    }

}


