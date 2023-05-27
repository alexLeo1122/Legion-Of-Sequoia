


namespace RPG.Character
{
    public abstract class CharacterBaseStateMachine
    {
        //Current State
        //Dictionary of states
        //Constructor
        public abstract void Initialize();
        //TransitionToState for eacht type of Character
        public abstract void Update();

    }
}

