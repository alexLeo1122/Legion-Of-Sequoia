using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class PlayerHealth : CharacterHealth
    {
        public float MaxHealth { get; private set; }
        private PlayerController playerController;
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }
        public override void OnHealthDepleted()
        {
            if (playerController.StateMachine.CurrentState is PlayerDefeatedState) return;
            playerController.PlayerActionsSO.OnPlayerDefeatedRaised();
            playerController.StateMachine.TransitionToState(PlayerStateEnum.PlayerDefeatedState);
        }
        public void SetMaxHealth(float value)
        {
            MaxHealth = value;
        }
        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthRatioChangedRaised();
        }
        //public override void TakeDamage(float damage)
        //{
        //    base.TakeDamage(damage);
        //    //GlobalEventManager.OnPlayTakeDamageEffectRaised();
        //}
    }
}

