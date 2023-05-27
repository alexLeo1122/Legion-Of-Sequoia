using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Quest
{
    public class FoxIdleState : FoxBaseState
    {
        public FoxIdleState(FoxController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
            controller.isWaitToChase = false;
        }

        public override void Update()
        {
            if (controller.IsDefeated)
            {
                controller.StateMachine.TransitionToState(FoxStateEnum.FoxDefeatedState);
                return;
            }
            if (controller.IsAttacking) return;
            if (controller.IsPlayerInChaseRange && !controller.isWaitToChase)
            {
                controller.isWaitToChase = true;
                controller.StateMachine.TransitionToState(FoxStateEnum.FoxChasingState);
            }
        }
    }
}


