using RPG.Character;

namespace RPG.Quest
{
    public abstract class FoxBaseState : ICharacterState
    {
        protected FoxController controller;

        public FoxBaseState(FoxController controller)
        {
            this.controller = controller;
        }
        public abstract void Enter();


        public abstract void Exit();


        public abstract void Update();
  
    }
}

