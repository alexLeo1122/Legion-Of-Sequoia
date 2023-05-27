
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyChaseState : EnemyBaseState
    {
        public EnemyChaseState(EnemyController controller) : base(controller) { }
        public override void Enter()
        {
            enemyController.MovementCmp.SetAgentSpeed(enemyController.EnemyStatSO.runSpeed);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            if (DistanceToPlayer <= enemyController.EnemyStatSO.attackRange)
            {
               enemyController.MovementCmp.StopAgent();
               enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyAttackState);
            }
            else if (DistanceToPlayer <= enemyController.EnemyStatSO.chaseRange + 0.1f)
            {
               enemyController.ChasePlayer();
            }
            //if DistanceToPlayer > ChaseRange, Transition to IdleState
            else if (DistanceToPlayer > enemyController.EnemyStatSO.chaseRange + 0.1f)
            {
               enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyIdleState);
            }

        }
    }

}