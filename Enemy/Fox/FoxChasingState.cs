using RPG.Character;
using RPG.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Quest
{
    public class FoxChasingState : FoxBaseState
    {
        public FoxChasingState(FoxController controller) : base(controller)
        {
        }
        public override void Enter()
        {
            controller.FoxVisual.HandleFoxRunAnim();
        }
        public override void Exit()
        {
            controller.FoxVisual.HandleFoxIdleAnim();
        }
        public override void Update()
        {
            if (controller.IsDefeated)
            {
                controller.StateMachine.TransitionToState(FoxStateEnum.FoxDefeatedState);
                return;
            }
            if (controller.IsAttacking) return;
            Vector3 directionVector = PlayerController.Instance.transform.position - controller.transform.position;
            controller.RotateTowardTarget(directionVector);
            controller.ChasePlayer();
        }
    }
}


