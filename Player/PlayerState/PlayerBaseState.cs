
namespace RPG.Character
{
    public abstract class PlayerBaseState : ICharacterState
    {
        protected PlayerController playerController;
        public PlayerBaseState(PlayerController controller)
        {
            this.playerController = controller;
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}


