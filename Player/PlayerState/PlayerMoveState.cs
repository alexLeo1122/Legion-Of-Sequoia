
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
            //playerController.ResetMovementVector();
        }

        public override void Update()
        {

            if (playerController.MovementVector == Vector3.zero)
            {
                playerController.StateMachine.TransitionToState(PlayerStateEnum.PlayerIdleState);
            }
            else
            {
                playerController.MovePlayer();
            }
            //Transition to attack state

        }
    }
}

