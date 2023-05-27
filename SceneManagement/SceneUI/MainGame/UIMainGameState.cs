using RPG.Character;
using UnityEngine;

namespace RPG.UI
{
    public class UIMainGameState : UIBaseState
    {
        public UIMainGameState(UIController controller) : base(controller)
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Enable();
        }

        public override void Exit()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Disable();
        }

        public override void SelectButton()
        {
           
        }
    }

}

