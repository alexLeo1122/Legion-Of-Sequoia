
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyAttackState : EnemyBaseState
    {
        public EnemyAttackState(EnemyController controller) : base(controller) { }
        public override void Enter()
        {
        }
        public override void Exit()
        {
        }
        public override void Update()
        {
            //if DistanceToPlayer > AttackRange, Transition to ChaseState
            if (DistanceToPlayer > enemyController.EnemyStatSO.attackRange)
            {
                enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyChaseState);
            }
            else
            {
                //if Enemy is Attacking  (Enemy is in Attack Animation) => return
                if (enemyController.ActionsRecord.isAttacking) return;
                enemyController.AttackPlayer();

            }
        }
    }
}

