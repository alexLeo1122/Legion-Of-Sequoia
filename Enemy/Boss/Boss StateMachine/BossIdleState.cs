using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Ultilities;
using UnityEngine;

namespace RPG.Character
{

    public class BossIdleState : BossBaseState
    {

        public BossIdleState(BossController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            bossController.MovementCmp.StopAgent();
        }

        public override void Exit()
        {
            bossController.hasResetSpeedBlend = false;
        }

        public override void Update()
        {


            if (DistanceToPlayer <= bossController.BossStatSO.chaseRange)
            {
                bossController.StateMachine.TransitionToState(BossStateEnum.BossChaseState);
            }

            // //if DistanceToPlayer < ChaseRange, Transition to ChaseState
            // if (DistanceToPlayer <= bossController.BossStatSO.chaseRange)
            // {
            //     bossController.StateMachine.TransitionToState(BossStateEnum.BossChaseState);
            // }
            // //if DistanceToPlayer < AttackRange, Transition to AttackState
            // else if (DistanceToPlayer <= bossController.BossStatSO.attackRange)
            // {
            //     bossController.StateMachine.TransitionToState(BossStateEnum.BossAttackState);
            // }
            // else if (DistanceToPlayer > bossController.BossStatSO.chaseRange)
            // {
            //     if (bossController.ActionsRecord.OriginalPosition == Vector3.zero)
            //     {
            //         return;
            //     }
            //     //Return to Original Position
            //     if (bossController.transform.position != bossController.ActionsRecord.OriginalPosition)
            //     {
            //         bossController.StateMachine.TransitionToState(BossStateEnum.BossReturningState);
            //     }
            // }

        }
    }
}




