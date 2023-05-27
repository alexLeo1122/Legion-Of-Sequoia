using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Ultilities;
using UnityEngine;

namespace RPG.Character
{
    public class PlayerIdleState : PlayerBaseState
    {

        public PlayerIdleState(PlayerController controller) : base(controller)
        {

        }

        public override void Enter()
        {
            playerController.PlayerActionsSO.OnPlayerIdleRaised();
        }

        public override void Exit()
        {
            playerController.hasResetSpeedBlend = false;
        }

        public override void Update()
        {
            if (playerController.MovementVector != Vector3.zero)
            {
                playerController.StateMachine.TransitionToState(PlayerStateEnum.PlayerMoveState);
            }
        }
    }
}

