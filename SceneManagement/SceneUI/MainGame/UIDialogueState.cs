using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Ink.Runtime;
using RPG.Character;
using RPG.Ultilities;

namespace RPG.UI
{
    public class UIDialogueState : UIBaseState
    {
        #region Declarations
        private VisualElement dialogueContainer;
        private Label dialogueText;
        private VisualElement nextButton;
        private VisualElement choicesGroup;
        private bool isDialoguehasChoices => currentStory.currentChoices.Count > 0;
        private Story currentStory;
        public Story CurrentStory { 
            get { return currentStory; }
            set { currentStory = value; }
        }       
        private List<Button> buttonsList => controller.ButtonsList;
        public UIDialogueState(UIController controller) : base(controller)
        {
            this.controller = controller;
        }
        #endregion

        #region Inherited Methods
        public override void Enter() 
        {
            PlayerController.Instance.GameInputSO.GameInput.UI.Enable();
            dialogueContainer = controller.Root.Q<VisualElement>("dialogue-container");
            dialogueText = dialogueContainer.Q<Label>("dialogue-text");
            nextButton = dialogueContainer.Q<VisualElement>("dialogue-next-button");
            choicesGroup = dialogueContainer.Q<VisualElement>("choices-group");
            dialogueContainer.style.display = DisplayStyle.Flex;
            PlayerController.Instance.SwitchGameInputActionMaps();
            GlobalEventManager.OnHidingHealthBarUIRaised(PlayerController.Instance.HealthCmp);
        }
        
        public override void SelectButton()
        {
            UpdateStory();
        }
        public override void Exit() 
        {
            PlayerController.Instance.GameInputSO.GameInput.UI.Disable();
        }
        #endregion

        #region Custom Methods
        public void SetStory(TextAsset inkText) 
        {
            currentStory = new Story(inkText.text);
            UpdateStory();
        }
        public void UpdateStory()
        {
            if (!currentStory.canContinue)
            {
                ExitDialogue();
                return;
            }
            dialogueText.text = currentStory.Continue();

            //Choice Handling
            if (isDialoguehasChoices)
            {
                currentStory.ChooseChoiceIndex(controller.SelectedButton);
            }
            if (isDialoguehasChoices)
            {
                DisplayChoices();
            }
            else
            {
                HideChoices();
            }
        }
        public void DisplayChoices()
        {
            nextButton.style.display = DisplayStyle.None;
            HandleNewChoices(currentStory.currentChoices);
            choicesGroup.style.display = DisplayStyle.Flex;
            controller.SetSelectedButton(0);
        }
        public void HideChoices()
        {
            buttonsList.Clear();
            choicesGroup.style.display = DisplayStyle.None;
            nextButton.style.display = DisplayStyle.Flex;
        }
        public void HandleNewChoices(List<Choice> newChoices)
        {
            choicesGroup.Clear();
            buttonsList.Clear();
            newChoices.ForEach(CreateNewButtons);
        }

        private void CreateNewButtons(Choice choice)
        {
            Button newButton = new Button();
            newButton.AddToClassList("choice-button");
            newButton.text = choice.text;
            choicesGroup.Add(newButton);
            buttonsList.Add(newButton);
        }
        private void ExitDialogue()
        {
            buttonsList.Clear();
            dialogueContainer.style.display = DisplayStyle.None;
            var hasMissionAchieved = (bool) currentStory.variablesState[GameConstants.HasMissionAchievedInkVar];
            GlobalEventManager.OnCurrentDialogueEndRaised(hasMissionAchieved);
            controller.TransitionToState(controller.uiMainGameState);
            GlobalEventManager.OnShowingHealthBarUIRaised(PlayerController.Instance.HealthCmp);
        }
        #endregion
    }
}

