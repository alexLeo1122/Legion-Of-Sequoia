using UnityEngine;
using RPG.Core;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using RPG.Character;



namespace RPG.UI
{
    public class StartSceneUIController : MonoBehaviour
    {
        [SerializeField] private GameSaveManager gameSaveManager;
        private GameInputManager inputManager;
        public VisualElement Root { get; private set; }
        public List<Button> ButtonsList { get; set; } = new List<Button>();
        private VisualElement choicesGroup;


        public int SelectedButton { get; set; } = 0;

        private void Awake()
        {
            inputManager = GetComponent<GameInputManager>();
        }
        private void Start()
        {
            InitializeInput();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            choicesGroup = Root.Q<VisualElement>("choices-group");
            var menuButtonsQuery = choicesGroup.Query<Button>().Class("character-button");
            menuButtonsQuery.ForEach(button => ButtonsList.Add(button));
            SetSelectedButton(0);

        }
        private void InitializeInput()
        {
            inputManager.GameInputSO.GameInput.UI.Disable();
            inputManager.GameInputSO.GameInput.UI.Enable();
            inputManager.GameInputSO.GameInput.UI.Interact.started += UIInteract_started;
            inputManager.GameInputSO.GameInput.UI.Navigate.started += UINavigate_started;
        }
        private void OnDisable()
        {
            inputManager.GameInputSO.CleanUp();
            inputManager.GameInputSO.GameInput.UI.Interact.started -= UIInteract_started;
            inputManager.GameInputSO.GameInput.UI.Navigate.started -= UINavigate_started;
        }

        private void UINavigate_started(InputAction.CallbackContext context)
        {
            if (ButtonsList.Count <= 1) return;
            ButtonsList[SelectedButton].RemoveFromClassList("active-character-button");
            Vector2 inputVector = context.ReadValue<Vector2>();
            SelectedButton += inputVector.x > 0 ? 1 : -1;
            SelectedButton = Mathf.Clamp(SelectedButton, 0, ButtonsList.Count - 1);
            ButtonsList[SelectedButton].AddToClassList("active-character-button");
        }

        private void UIInteract_started(InputAction.CallbackContext context)
        {
            gameSaveManager.HeroChoice.SetHeroChoice(SelectedButton);
            SceneManager.LoadScene(2);

        }
        public void SetSelectedButton(int index)
        {
            SelectedButton = index;
            ButtonsList[SelectedButton].AddToClassList("active-character-button");
        }

    }
}






