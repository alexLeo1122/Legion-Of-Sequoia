using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyStateMachine : CharacterBaseStateMachine
    {
        public EnemyBaseState CurrentState { get; private set; }
        public Dictionary<string, EnemyBaseState> stateDict;
        private EnemyController enemyController;
        public EnemyStateMachine(EnemyController controller)
        {
            this.enemyController = controller;
            stateDict = new Dictionary<string, EnemyBaseState>();
            //get assembly
            Assembly assembly = Assembly.GetAssembly(typeof(EnemyStateMachine));
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(EnemyBaseState)));
            //get all types that are a subclass of EnemyBaseState,->
            //-> create instance of each type and add to dictionary
            foreach (var type in types)
            {
                //Get constructor of each type that has parameter of EnemyController
                ConstructorInfo constructorInfo = type.GetConstructor(new Type[] { typeof(EnemyController) });
                var state = (EnemyBaseState)constructorInfo.Invoke(new object[] { controller });
                if (stateDict.ContainsKey(state.GetType().Name))
                {
                    continue;
                }
                stateDict.Add(state.GetType().Name, state);
            }
        }
        public override void Initialize()
        {
            TransitionToState(EnemyStateEnum.EnemyIdleState);
        }
        public void TransitionToState(EnemyStateEnum state)
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
                TransitionToState(EnemyStateEnum.EnemyIdleState);
                return;
            }
            if (CurrentState is not EnemyPatrolState)
            {
                enemyController.RotateAgent(enemyController.DirectionVector);

            }
            CurrentState.Update();
        }
    }
}



