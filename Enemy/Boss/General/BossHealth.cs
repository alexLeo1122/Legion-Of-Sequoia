using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class BossHealth : CharacterHealth
    {
        public float MaxHealth { get; private set; }

        private BossController bossController;
        private void Awake()
        {
            bossController = GetComponent<BossController>();
        }
        private new void Start()
        {
            base.Start();
            BossEventManager.OnBossAppearedRaised(this);
        }

        public override void OnHealthDepleted()
        {
            if (bossController.StateMachine.CurrentState is BossDefeatedState) return;
            if (PlayerController.Instance.CombatCmp.enemyMonitor.healthList.Contains(this))
            {
                PlayerController.Instance.CombatCmp.enemyMonitor.healthList.Remove(this);
            }
            GlobalEventManager.OnGameObjectDisappearedRaised(transform);
            GetComponent<BossTeleport>().RemoveAgent();
            PlayerController.Instance.CombatCmp.TargetEnemy = null;
            bossController.gameObject.layer = LayerMask.NameToLayer(GameConstants.EnemyDefeatedLayer);
            bossController.StateMachine.TransitionToState(BossStateEnum.BossDefeatedState);
            GlobalEventManager.OnSkelMageBeingDefeatedRaised();
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
        public void TeleportToPos()
        {
            transform.position = bossController.MovementCmp.TeleportPos.position;

        }
    }

}

