using RPG.Character;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace RPG.UI
{
    public class UIResultState : UIBaseState
    {
        private VisualElement resultContainer;
        private VisualElement winGroup;
        private VisualElement loseGroup;
        private VisualElement restartButton;
        public bool HasPlayerWon { get; set; } = false;


        public UIResultState(UIController controller) : base(controller)
        {
            this.controller = controller;
        }

        public override void Enter()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Disable();
            PlayerController.Instance.GameInputSO.GameInput.UI.Enable();
            resultContainer = controller.Root.Q<VisualElement>("game-result-container");
            winGroup = resultContainer.Q<VisualElement>("win-group");
            loseGroup = resultContainer.Q<VisualElement>("lose-group");
            restartButton = resultContainer.Q<VisualElement>("restart-button");
            resultContainer.style.display = DisplayStyle.Flex;
            if (HasPlayerWon)
            {
                winGroup.style.display = DisplayStyle.Flex;
                loseGroup.style.display = DisplayStyle.None;
            }
            else
            {
                winGroup.style.display = DisplayStyle.None;
                loseGroup.style.display = DisplayStyle.Flex;
            }
        }

        public override void Exit()
        {
            HasPlayerWon = false;
            winGroup.style.display = DisplayStyle.None;
            loseGroup.style.display = DisplayStyle.None;
            resultContainer.style.display = DisplayStyle.None;
            PlayerController.Instance.GameInputSO.GameInput.Player.Enable();
            PlayerController.Instance.GameInputSO.GameInput.UI.Disable();
        }

        public override void SelectButton()
        {
            GlobalEventManager.HasGameRestarted = true;
            SceneManager.LoadScene(0);
        }
    }
}


