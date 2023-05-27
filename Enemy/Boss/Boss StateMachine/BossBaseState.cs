
using UnityEngine;

namespace RPG.Character
{
    public abstract class BossBaseState : ICharacterState
    {
        protected BossController bossController;

        protected float DistanceToPlayer
        {
            get
            {
                return bossController.DistanceToTarget(PlayerController.Instance.transform.position);
            }
        }
        public BossBaseState(BossController controller)
        {
            this.bossController = controller;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}
