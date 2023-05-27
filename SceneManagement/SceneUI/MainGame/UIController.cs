using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using RPG.Character;
using Ink.Runtime;
using System.Collections.Generic;
using RPG.Ultilities;

namespace RPG.UI
{
    public class UIController : MonoBehaviour
    {
        public VisualElement Root { get; private set; }
        private UIBaseState currentUIState;
        public UIBaseState uiDialogueState;
        public UIBaseState uiMainGameState;
        public UIBaseState uiResultState;
        public List<Button> ButtonsList { get; set; } = new List<Button>();
        public int SelectedButton { get; set;  } = 0;

        private void Awake()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            uiDialogueState = new UIDialogueState(this);
            uiMainGameState = new UIMainGameState(this);
            uiResultState = new UIResultState(this);
            GlobalEventManager.OnInitiateDialogue += HandleOnInitiateDialogue;
            GlobalEventManager.OnResetStory += GlobalEventManager_OnResetStory;
            GlobalEventManager.OnGameEnd += GlobalEventManager_OnGameEnd;
            //PlayerController.Instance.GameInputSO.GameInput.UI.Interact.started += HandleUIInteract_started;
            //PlayerController.Instance.GameInputSO.GameInput.UI.Navigate.started += HandleUINavigate_started;

        }

        private void GlobalEventManager_OnGameEnd(object sender, bool result)
        {
            (uiResultState as UIResultState).HasPlayerWon = result;
            TransitionToState(uiResultState);
        }

        private void GlobalEventManager_OnResetStory(object sender, GlobalEventManager.OnResetStoryArgs args)
        {
            var resetStory = new Story(args.dialogueFile.text); 
            if (!(bool)resetStory.variablesState[GameConstants.HasMissionAchievedInkVar])
            {
                resetStory.variablesState[GameConstants.HasMissionAchievedInkVar] = args.hasMissionAchieved;
            }
            TransitionToState(uiDialogueState);
            (currentUIState as UIDialogueState).CurrentStory = resetStory;
            (currentUIState as UIDialogueState).UpdateStory();
        }

        private void Start()
        {
            Initialize();
            PlayerController.Instance.GameInputSO.GameInput.UI.Interact.started += HandleUIInteract_started;
            PlayerController.Instance.GameInputSO.GameInput.UI.Navigate.started += HandleUINavigate_started;
        }

        private void OnDisable()
        {
            GlobalEventManager.OnInitiateDialogue -= HandleOnInitiateDialogue;
            GlobalEventManager.OnResetStory -= GlobalEventManager_OnResetStory;
            GlobalEventManager.OnGameEnd -= GlobalEventManager_OnGameEnd;
            PlayerController.Instance.GameInputSO.GameInput.UI.Interact.started -= HandleUIInteract_started;
            PlayerController.Instance.GameInputSO.GameInput.UI.Navigate.started -= HandleUINavigate_started;
        }

        public void SetSelectedButton(int index)
        {
            SelectedButton = index;
            ButtonsList[SelectedButton].AddToClassList("active-button");
        }
        public void TransitionToState(UIBaseState newState)
        {
            if (currentUIState != null)
            {
                currentUIState.Exit();
            }
            currentUIState = newState;
            currentUIState.Enter();
        }
        private void HandleOnInitiateDialogue(object sender, TextAsset textAsset)
        {
            currentUIState =  uiDialogueState;
            currentUIState.Enter();
            (currentUIState as UIDialogueState).SetStory(textAsset);
        }

        private void HandleUINavigate_started(InputAction.CallbackContext context)
        {
            if (ButtonsList.Count <= 1) return;
            ButtonsList[SelectedButton].RemoveFromClassList("active-button");
            Vector2 inputVector = context.ReadValue<Vector2>();
            SelectedButton += inputVector.x > 0 ? 1 : -1;
            SelectedButton = Mathf.Clamp(SelectedButton, 0, ButtonsList.Count - 1);
            ButtonsList[SelectedButton].AddToClassList("active-button");
        }

        private void HandleUIInteract_started(InputAction.CallbackContext context)
        {
            currentUIState.SelectButton();
        }
        private void Initialize()
        {
            currentUIState = uiDialogueState;
        }

    }
}


