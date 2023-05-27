using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public class BossStateMachine : CharacterBaseStateMachine
    {
        public BossBaseState CurrentState { get; private set; }

        public Dictionary<String, BossBaseState> stateDict;
        public BossController bossController;
        public BossStateMachine(BossController bossController)
        {
            this.bossController = bossController;
            stateDict = new Dictionary<String, BossBaseState>();
            //get assembly
            Assembly assembly = Assembly.GetAssembly(typeof(BossStateMachine));
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(BossBaseState)));
            //get all types that are a subclass of BossBaseState
            //-> create instance of each type and add to dictionary
            foreach (var type in types)
            {
                //Get constructor of each type that has parameter of BossController
                ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(BossController) });
                var state = (BossBaseState)constructorInfo.Invoke(new object[] { bossController });
                if (stateDict.ContainsKey(state.GetType().Name))
                {
                    continue;
                }
                stateDict.Add(state.GetType().Name, state);
            }

        }
        public override void Initialize()
        {
            TransitionToState(BossStateEnum.BossIdleState);
        }

        public void TransitionToState(BossStateEnum state)
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
            //Enemy stays Idle if player is defeated

            if (CurrentState != null && PlayerController.Instance.StateMachine.CurrentState is PlayerDefeatedState)
            {
                TransitionToState(BossStateEnum.BossIdleState);
                return;
            }

            CurrentState.Update();
        }
    }
}

