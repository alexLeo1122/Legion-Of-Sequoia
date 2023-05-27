
using UnityEngine;
using RPG.Ultilities;



namespace RPG.Character
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(EnemyController controller) : base(controller) { }
        public override void Enter()
        {
            enemyController.MovementCmp.StopAgent();
        }
        public override void Exit()
        {
            enemyController.hasResetSpeedBlend = false;
        }
        public override void Update()
        {
            if (enemyController.HasPatrolComponent() && 
                enemyController.transform.position == enemyController.ActionsRecord.OriginalPosition)
            {
                enemyController.PatrolCmp.Initiallize();
            }

            //if DistanceToPlayer < ChaseRange, Transition to ChaseState
            if (DistanceToPlayer <= enemyController.EnemyStatSO.chaseRange)
            {
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyChaseState);
            }
            //if DistanceToPlayer < AttackRange, Transition to AttackState
            else if (DistanceToPlayer <= enemyController.EnemyStatSO.attackRange)
            {
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyAttackState);
            }
            else if (DistanceToPlayer > enemyController.EnemyStatSO.chaseRange)
            {
                if (enemyController.ActionsRecord.OriginalPosition == Vector3.zero)
                {
                    return;
                }
                //Return to Original Position
                if (enemyController.transform.position != enemyController.ActionsRecord.OriginalPosition)
                {
                    enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyReturningState);
                }
            }


        }

    }
}

