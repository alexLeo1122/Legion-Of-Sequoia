using RPG.Character;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Quest
{
    public class FoxHealth : CharacterHealth
    {
        private float defaultHealth = 1f;
        private FoxController controller;
        private void Awake()
        {
            controller = GetComponent<FoxController>();
            CurrentHealth = defaultHealth;
        }
        public override void OnHealthDepleted()
        {
            if (controller.IsDefeated) return;
            controller.IsDefeated = true;
            //remove this enemy from enemyMonitor list, also changelayer to prevent player from attacking this enemy
            PlayerController.Instance.CombatCmp.enemyMonitor.healthList.Remove(this);
            PlayerController.Instance.CombatCmp.TargetEnemy = null;
            this.gameObject.layer = LayerMask.NameToLayer(GameConstants.EnemyDefeatedLayer);
            controller.StateMachine.TransitionToState(FoxStateEnum.FoxDefeatedState);
            GlobalEventManager.OnFoxBeingDefeatedRaised();
        }
    }
}
