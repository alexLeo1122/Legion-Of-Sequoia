using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    public class BossReturningState : BossBaseState
    {
        public BossReturningState(BossController controller) : base(controller) { }


        public override void Enter()
        {
            bossController.MovementCmp.SetAgentSpeed(bossController.BossStatSO.defaultSpeed);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
            ///////////// Only if player leave battle field



        }

    }
}


