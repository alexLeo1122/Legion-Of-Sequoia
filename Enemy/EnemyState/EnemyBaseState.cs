
namespace RPG.Character
{
    public abstract class EnemyBaseState : ICharacterState
    {
        protected EnemyController enemyController;
        protected float DistanceToPlayer
        {
            get
            {
                return enemyController.DistanceToTarget(PlayerController.Instance.transform.position);
            }
        }
        public EnemyBaseState(EnemyController controller)
        {
            this.enemyController = controller;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}

