using System;
using System.Collections;
using UnityEngine;

namespace RPG.Character
{
    public abstract class CharacterHealth : MonoBehaviour
    {
        #region Declarations
        public float CurrentHealth { get; protected set; }
        public float originalHealth { get; protected set; }
        public float HealthRatio => CurrentHealth / originalHealth;
        public float HealthDecrementTime { get; set; }
        #endregion
        #region Unity Methods
        private void OnEnable()
        {
            OnHealthComponentAppeared?.Invoke(this, new OnHealthComponentAppearedArgs { characterController = GetComponent<GameCharacterController>(), healthCmp = this }); ;
        }

        public void Start()
        {
            originalHealth = CurrentHealth;
            OnHealthComponentAppeared?.Invoke(this, new OnHealthComponentAppearedArgs { characterController = GetComponent<GameCharacterController>(), healthCmp = this }); ;
        }

        public void OnDisable()
        {
            OnRemoveHealthUI?.Invoke(this, new OnRemoveHealthUIArgs { characterHealth = this });
        }
        #endregion

        #region Event and Event Args
        public static event EventHandler<OnHealthComponentAppearedArgs> OnHealthComponentAppeared;
        public static event EventHandler<OnHealthRatioChangedArgs> OnHealthRatioChanged;
        public static event EventHandler<OnRemoveHealthUIArgs> OnRemoveHealthUI;
        public class OnHealthComponentAppearedArgs : EventArgs
        {
            public GameCharacterController characterController;
            public CharacterHealth healthCmp;
        }
        public class OnHealthRatioChangedArgs : EventArgs
        {
            public CharacterHealth characterHealth;
            public float healthRatio;
        }
        public class OnRemoveHealthUIArgs : EventArgs
        {
            public CharacterHealth characterHealth;
        }

        #endregion

        #region Raise-Event Method
        public void OnHealthComponentAppearedRaised(object sender, OnHealthComponentAppearedArgs args)
        {
            OnHealthComponentAppeared?.Invoke(sender, args);
        }
        public void OnHealthRatioChangedRaised()
        {
            OnHealthRatioChanged?.Invoke(this, new OnHealthRatioChangedArgs { healthRatio = HealthRatio, characterHealth = this });
        }

        #endregion

        public virtual void TakeDamage(float damage)
        {
            if (CurrentHealth <= 0f) return;
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);
            OnHealthRatioChangedRaised();
            var characterController = GetComponent<GameCharacterController>();
            GlobalEventManager.OnPlayTakeDamageEffectRaised(
                characterController
                );
            if (CurrentHealth == 0)
            {
                StartCoroutine(DelayThenRemoveCharacter(HealthDecrementTime));
            }
        }
        public abstract void OnHealthDepleted();
        public void SetCurrentHealth(float value)
        {
            CurrentHealth = value;
        }
        public void SetOriginalHealth(float value)
        {
            originalHealth = value;
        }
        public IEnumerator DelayThenRemoveCharacter(float time)
        {
            yield return new WaitForSeconds(time);
            OnRemoveHealthUI?.Invoke(this, new OnRemoveHealthUIArgs() { characterHealth = this }); ;
            OnHealthDepleted();
        }

    }

}
