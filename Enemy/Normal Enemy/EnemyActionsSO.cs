using System;
using UnityEngine;


namespace RPG.Character
{
    //This EnemyActionsSO act as Global Event to notify Actions to EnemyVisual.cs
    //unlike PlayerActionsSO, This will only notify Actions based on Enemy's State
    [CreateAssetMenu(fileName = "EnemyActionsSO", menuName = "ScriptableObjects/EnemyActionsSO")]

    public class EnemyActionsSO : ScriptableObject
    {
        public event EventHandler OnEnemyIdle;
        public event EventHandler OnEnemyReturning;
        public event EventHandler OnEnemyPatrol;
        public event EventHandler OnEnemyChase;
        public event EventHandler OnEnemyDefaultAttack;
        public event EventHandler OnEnemyHeavyAttack;
        public event EventHandler OnEnemyDefeated;
        public event EventHandler<OnEnemyMoveChangedArgs> OnEnemyMoveChanged;

        //----------------

        public class OnEnemyMoveChangedArgs: EventArgs
        {
            public float moveSpeedBlend;
        }
        //----------------

        public void OnEnemyMovedChangedRaised(object sender, OnEnemyMoveChangedArgs args)
        {
            OnEnemyMoveChanged?.Invoke(sender, args);
        }
        public void OnEnemyIdleRaised()
        {
            OnEnemyIdle?.Invoke(this, EventArgs.Empty);
        }
        public void OnEnemyReturningRaised()
        {
            OnEnemyReturning?.Invoke(this, EventArgs.Empty);
        }
        //patrol raised
        public void OnEnemyPatrolRaised()
        {
            OnEnemyPatrol?.Invoke(this, EventArgs.Empty);
        }
        public void OnEnemyChaseRaised()
        {
            OnEnemyChase?.Invoke(this, EventArgs.Empty);
        }
        public void OnEnemyDefaultAttackRaised()
        {
            OnEnemyDefaultAttack?.Invoke(this, EventArgs.Empty);
        }

        public void OnEnemyHeavyAttackRaised()
        {
            OnEnemyHeavyAttack?.Invoke(this, EventArgs.Empty);
        }

        public void OnEnemyDefeatedRaised()
        {
            OnEnemyDefeated?.Invoke(this, EventArgs.Empty);
        }


    }

}

