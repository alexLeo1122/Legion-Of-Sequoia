using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using RPG.Ultilities;
using UnityEngine;

namespace RPG.Character
{
    public class PlayerStateMachine : CharacterBaseStateMachine
    {
        public PlayerBaseState CurrentState { get; private set; }
        public Dictionary<string, PlayerBaseState> stateDict;
        public PlayerStateMachine(PlayerController controller)
        {
            stateDict = new Dictionary<string, PlayerBaseState>();
            //get assembly
            Assembly assembly = Assembly.GetAssembly(typeof(PlayerStateMachine));
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(PlayerBaseState)));
            //get all types that are a subclass of CharacterBaseState
            foreach (var type in types)
            {
                ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(PlayerController) });
                var state = (PlayerBaseState)constructorInfo.Invoke(new object[] { controller });
                if (stateDict.ContainsKey(state.GetType().Name))
                {
                    continue;
                }
                stateDict.Add(state.GetType().Name, state);
            }

        }
        public override void Initialize()
        {
            TransitionToState(PlayerStateEnum.PlayerIdleState);
        }
        public void TransitionToState(PlayerStateEnum state)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            var tempState = state.ToString();
            if (stateDict.ContainsKey(tempState))
            {
                CurrentState = stateDict[tempState];
            }
            else
            {
                Debug.LogError("State not found");
            }
            CurrentState.Enter();
        }
        public override void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
    }

}

