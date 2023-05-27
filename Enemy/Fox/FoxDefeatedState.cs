using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    public class FoxDefeatedState : FoxBaseState
    {
        public FoxDefeatedState(FoxController controller) : base(controller)
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
            if (!controller.IsDefeatedAnimEnd) return;
            if (controller.IsWaitBeforeRun) return;
            controller.EnemyWaitAndRun();
            if (!controller.IsFinishWaiting) return;
            controller.RunbackToCave();
        }
    }
}



