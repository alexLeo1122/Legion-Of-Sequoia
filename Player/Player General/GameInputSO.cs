using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Ultilities;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "GameInputSO", menuName = "ScriptableObjects/GameInputSO", order = 0)]
    public class GameInputSO : ScriptableObject
    {
        #region Declarations
        //---Start of Declarations
        public Vector3 MovementVector { get; private set; }
        private GameInput gameInput;
        public GameInput GameInput => gameInput;
        public event Action OnMovementVectorUpdated;
        public Dictionary<InputAction, PlayerAbilityEnum> ActionsDict { get; private set; }
        //---End of Declarations
        #endregion

        #region Custom Methods
        //---Start of Custom Methods
        public void Initialize()
        {
            gameInput = new GameInput();
            ActionsDict = new Dictionary<InputAction, PlayerAbilityEnum>();
            gameInput.Player.Movement.performed += OnPlayerMovement_performed;
            gameInput.Player.Movement.canceled += OnPlayerMovement_canceled;
            gameInput.Player.Enable();

            foreach (var actionEnum in Enum.GetValues(typeof(PlayerAbilityEnum)))
            {
                foreach (var action in gameInput.Player.Get()) {
                    if (action.name == actionEnum.ToString())
                    {
                        ActionsDict.Add(action, (PlayerAbilityEnum)actionEnum);
                    }
                }
            }
        }

        public void CleanUp()
        {
            gameInput.Player.Movement.performed -= OnPlayerMovement_performed;
            gameInput.Player.Movement.canceled -= OnPlayerMovement_canceled;
            gameInput.Player.Disable();
            //gameInput.UI.Disable();
        }

        //---End of Custom Methods
        #endregion

        #region Event Handlers
        //---Start of Event Handlers

        private void OnPlayerMovement_canceled(InputAction.CallbackContext context)
        {
            UpdateMovementVector(context);
        }
        private void OnPlayerMovement_performed(InputAction.CallbackContext context)
        {
            UpdateMovementVector(context);
        }
        private void UpdateMovementVector(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            MovementVector = new Vector3(inputVector.x, 0f, inputVector.y);
            OnMovementVectorUpdated?.Invoke();
        }
        //---End of Event Handlers
        #endregion

    }
}

