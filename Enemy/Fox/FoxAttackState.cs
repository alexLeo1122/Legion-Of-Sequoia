
using RPG.Ultilities;
using UnityEngine;


namespace RPG.Quest
{
    public class FoxAttackState : FoxBaseState
    {
        public FoxAttackState(FoxController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            controller.StopAgent();
        }

        public override void Exit()
        {
            //controller.IsAttacking = false; 
        }

        public override void Update()
        {
            if (controller.IsDefeated)
            {
                controller.StateMachine.TransitionToState(FoxStateEnum.FoxDefeatedState);
                return;
            }
            if (controller.IsAttacking) return;
            controller.AttackPlayer();
            controller.FoxVisual.HandleFoxAttackAnim();
            controller.IsAttacking = true;
        }
    }
}



