
using RPG.Ultilities;
using UnityEngine;


namespace RPG.Character
{
    public class EnemyPatrolState : EnemyBaseState
    {

        public EnemyPatrolState(EnemyController controller) : base(controller) { }
        public override void Enter()
        {
            //not setting agent speed, SplineAnimate takes care of that
        }
        public override void Exit()
        {
            enemyController.PatrolCmp.StopPatrolling();
            enemyController.PatrolCmp.IsResetSplinePath = false;
            enemyController.PatrolCmp.IsAlreadyPatrolling = false;
            

        }
        public override void Update()
        {
            //if DistanceToPlayer < ChaseRange, Transition to ChaseState
            if (DistanceToPlayer <  enemyController.EnemyStatSO.chaseRange)
            {
                enemyController.Agent.velocity = Vector3.zero;
                enemyController.MovementCmp.StopAgent();
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyChaseState);
            }
            //if DistanceToPlayer < AttackRange, Transition to AttackState
            else if (DistanceToPlayer <= enemyController.EnemyStatSO.attackRange)
            {
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyAttackState);
            }
            else if (DistanceToPlayer > enemyController.EnemyStatSO.chaseRange)
            {
                enemyController.PatrolCmp.Patrol();
            }



        }
    }
}






