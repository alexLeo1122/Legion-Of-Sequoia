
using System;
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    //This PlayerActionsSO act as Global Event to notify Actions to PlayerVisual.cs
    //This will not only notify Actions based on Player's input but also notify Actions based on Player's State
    [CreateAssetMenu(fileName = "PlayerActionsSO", menuName = "ScriptableObjects/PlayerActionsSO")]
    public class PlayerActionsSO : ScriptableObject
    {
        #region Events
        public event EventHandler OnPlayerIdle;
        public event EventHandler<OnPlayerMovedArgs> OnPlayerMoved;
        public event EventHandler<OnPlayerDefaultAttackArgs> OnPlayerDefaultAttack;
        public event EventHandler<OnPlayerHeavyAttackArgs> OnPlayerHeavyAttack;
        public event EventHandler<OnPlayerDashFrontAttackArgs> OnPlayerDashFrontAttack;
        public event EventHandler<OnPlayerHealingSpellArgs> OnPlayerHealingSpell;
        public event EventHandler<OnPlayerPickingUpArgs> OnPlayerPickingUp;
        public event EventHandler OnPlayerDefeated;
        #endregion

        #region Event Arguments

        public class OnPlayerMovedArgs : EventArgs
        {
            public float moveSpeedBlend;
        }
        public class OnPlayerDefaultAttackArgs : EventArgs
        {
            //public EnemyController targetController;
        }
        public class OnPlayerHeavyAttackArgs : EventArgs
        {
            //public EnemyController targetController;
        }
        public class OnPlayerDashFrontAttackArgs : EventArgs
        {
            //public EnemyController targetController;
        }
        public class OnPlayerHealingSpellArgs : EventArgs
        {
            //public EnemyController targetController;
        }
        public class OnPlayerPickingUpArgs : EventArgs
        {
            //public EnemyController targetController;
        }

        #endregion

        #region Raised Event Methods

        public void OnPlayerIdleRaised()
        {
            OnPlayerIdle?.Invoke(this, EventArgs.Empty);
        }
        public void OnPlayerDefeatedRaised()
        {
            OnPlayerDefeated?.Invoke(this, EventArgs.Empty);
        }
        public void OnPlayerMoveChangedRaised(object sender, OnPlayerMovedArgs args)
        {
            OnPlayerMoved?.Invoke(sender, args);
        }
        //---DefaultAttack
        [Ability(PlayerAbilityEnum.DefaultAttack)]
        public void OnPlayerDefaultAttackRaised()
        {
            OnPlayerDefaultAttack?.Invoke(this, new OnPlayerDefaultAttackArgs());
        }
        //---HeavyAttack

        [Ability(PlayerAbilityEnum.HeavyAttack)]
        public void OnPlayerHeavyAttackRaised()
        {
            OnPlayerHeavyAttack?.Invoke(this, new OnPlayerHeavyAttackArgs());
        }
        //--DashFrontAttack
        [Ability(PlayerAbilityEnum.DashFrontAttack)]
        public void OnPlayerDashFrontAttackRaised()
        {
            OnPlayerDashFrontAttack?.Invoke(this, new OnPlayerDashFrontAttackArgs());
        }

        //--HealingSpell
        [Ability(PlayerAbilityEnum.HealingSpell)]
        public void OnPlayerHealingSpellRaised()
        {
            OnPlayerHealingSpell?.Invoke(this, new OnPlayerHealingSpellArgs());
        }

        //--PickingUp
        [Ability(PlayerAbilityEnum.PickingUp)]
        public void OnPlayerPickingUpRaised()
        {
            OnPlayerPickingUp?.Invoke(this, new OnPlayerPickingUpArgs());
        }
        #endregion

    }

}


