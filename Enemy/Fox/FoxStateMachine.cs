using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using RPG.Character;
using RPG.Ultilities;

namespace RPG.Quest
{
    public class FoxStateMachine : CharacterBaseStateMachine
    {

        public FoxBaseState CurrentState { get; private set; }
        public Dictionary<string, FoxBaseState> stateDict;
        //modify EnemyStateMachine to use FoxController
        private FoxController foxController;
        public FoxStateMachine(FoxController controller)
        {
            this.foxController = controller;
            stateDict = new Dictionary<string, FoxBaseState>();
            Assembly assembly = Assembly.GetAssembly(typeof(FoxStateMachine));
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(FoxBaseState)));
            foreach (var type in types)
            {
                ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(FoxController) });
                var state = (FoxBaseState)constructorInfo.Invoke(new object[] { controller });
                if (stateDict.ContainsKey(state.GetType().Name))
                {
                    continue;
                }
                stateDict.Add(state.GetType().Name, state);
            }
        }

        public override void Initialize()
        {
            TransitionToState(FoxStateEnum.FoxIdleState);
        }

        public override void Update()
        {

            if (CurrentState != null && PlayerController.Instance.StateMachine.CurrentState is PlayerDefeatedState)
            {
                TransitionToState(FoxStateEnum.FoxIdleState);
                return;
            }
            CurrentState.Update();
            if (foxController.StateMachine.CurrentState is FoxDefeatedState) return;
            if (foxController.IsPlayerInAttackRange
                && CurrentState is not FoxAttackState)
            {
                TransitionToState(FoxStateEnum.FoxAttackState);
            }
        }

        public void TransitionToState(FoxStateEnum state)
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


    }
}

