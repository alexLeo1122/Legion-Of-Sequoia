
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyReturningState : EnemyBaseState
    {
        public EnemyReturningState(EnemyController controller) : base(controller) { }
        public override void Enter()
        {
            enemyController.MovementCmp.SetAgentSpeed(enemyController.EnemyStatSO.defaultSpeed);
        }
        public override void Exit()
        {
        }
        public override void Update()
        {
            // if enemy has not reached original position && player is in range of Chase or Attack
            if (DistanceToPlayer <= enemyController.EnemyStatSO.attackRange)
            {
                enemyController.MovementCmp.StopAgent();
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyAttackState);
                return;
            }
            else if (DistanceToPlayer <= enemyController.EnemyStatSO.chaseRange)
            {
                enemyController.MovementCmp.StopAgent();
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyChaseState);
                return;
            }
            //if player is not in range of Chase or Attack
                // if enemy has  reached original position, transite to idle state
            if (enemyController.transform.position == enemyController.ActionsRecord.OriginalPosition)
            {
                if (enemyController.transform.forward == enemyController.ActionsRecord.OriginalForwardDirection)
                {
                    enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyIdleState);
                }
                enemyController.SetDirectionVector(enemyController.ActionsRecord.OriginalForwardDirection);
            }
            else
                // if enemy has not reached original position, move to original position
            {
                Vector3 newDirectionVector = enemyController.ActionsRecord.OriginalPosition - enemyController.transform.position;
                enemyController.SetDirectionVector(newDirectionVector);
                enemyController.MovementCmp.MoveAgent(enemyController.ActionsRecord.OriginalPosition);
            }

        }
    }

}