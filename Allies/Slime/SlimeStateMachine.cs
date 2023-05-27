using System;
using RPG.Character;
using RPG.Ultilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections;



namespace RPG.Allies
{
    public class SlimeStateMachine : CharacterBaseStateMachine
    {
        public SlimeBaseState CurrentState { get; private set; }
        public Dictionary<SlimeStateEnum, SlimeBaseState> stateDict;
        public SlimeStateMachine(SlimeController controller)
        {
            stateDict = new Dictionary<SlimeStateEnum, SlimeBaseState>();
            Assembly assembly = Assembly.GetAssembly(typeof(SlimeStateMachine));
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(SlimeBaseState)));
            foreach ( var type in types)
            {
                var constructorInfo = type.GetConstructor(new Type[] {typeof(SlimeController)});
                var state = (SlimeBaseState) constructorInfo.Invoke(new object[] { controller});
                var stateEnum = type.GetCustomAttribute<SlimeStateAttribute>().stateEnum;
                if (stateDict.ContainsKey(stateEnum))
                {
                    continue;
                }
                stateDict.Add(stateEnum, state);
            }
        }
        public override void Initialize()
        {
            TransitionToState(SlimeStateEnum.SlimeFollowState);
        }
        public void TransitionToState(SlimeStateEnum state)
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            if (stateDict.ContainsKey(state))
            {
                CurrentState = stateDict[state];
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
