using RPG.Character;
using RPG.Ultilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Allies
{
    [SlimeState(SlimeStateEnum.SlimeFollowState)]
    public class SlimeFollowState : SlimeBaseState
    {
        public SlimeFollowState(SlimeController controller) : base(controller)
        {
        }

        public override void Enter()
        {
           
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            //only follow if player not attacking
            if (PlayerController.Instance.StateMachine.CurrentState is PlayerPerformAbilityState)
            {
                controller.StateMachine.TransitionToState(SlimeStateEnum.SlimePerformAbilityState);
            }
        }
    }
}

