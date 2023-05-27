namespace  RPG.UI
{
    public abstract class UIBaseState
    {
        public UIController controller;

        public UIBaseState(UIController controller)
        {
            this.controller = controller;   
        }
        public abstract void Enter();
        public abstract void SelectButton();
        public abstract void Exit();
    }
}


