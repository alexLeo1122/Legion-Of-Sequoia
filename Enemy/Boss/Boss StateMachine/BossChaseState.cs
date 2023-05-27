using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class BossChaseState : BossBaseState
    {


        public BossChaseState(BossController controller) : base(controller)
        {
        }
        public override void Enter()
        {
            bossController.MovementCmp.SetAgentSpeed(bossController.BossStatSO.runSpeed);

        }
        public override void Exit()
        {

        }

        public override void Update()
        {
            //             if (DistanceToPlayer <= enemyController.EnemyStatSO.attackRange)
            if (DistanceToPlayer <= bossController.BossStatSO.attackRange)
            {
                //Debug.Log("Chase to Attack");
                bossController.MovementCmp.StopAgent();
                bossController.StateMachine.TransitionToState(BossStateEnum.BossPerformAbilityState);
            }
            //if DistanceToPlayer <= ChaseRange, ChasePlayer
            else if (DistanceToPlayer <= bossController.BossStatSO.chaseRange)
            {
                bossController.ChasePlayer();
            }
        }
    }


}

