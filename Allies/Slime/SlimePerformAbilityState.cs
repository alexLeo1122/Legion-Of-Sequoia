using RPG.Character;
using RPG.Ultilities;
using UnityEngine;

namespace RPG.Allies
{
    [SlimeState(SlimeStateEnum.SlimePerformAbilityState)]
    public class SlimePerformAbilityState : SlimeBaseState
    {
        private float damage;
        private CharacterHealth target;
        public SlimePerformAbilityState(SlimeController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            damage = controller.DefaultDamage;
            controller.StopAgent();
        }

        public override void Exit()
        {
           target = null;
        }

        public override void Update()
        {
            if (controller.IsPlayerInRange)
            {
                target = PlayerController.Instance.CombatCmp.TargetEnemy;
                if (target == null
                    && PlayerController.Instance.StateMachine.CurrentState is not PlayerPerformAbilityState)
                {
                    controller.StateMachine.TransitionToState(SlimeStateEnum.SlimeFollowState);
                }
                if (target == null || controller.statusMonitor.isPerformAbility) return;
                controller.PerformAbility();
                GlobalEventManager.OnSlimePerformAbilityRaised(null,
                new GlobalEventManager.OnSlimePerformAblilityArgs() { damage = damage, target = target });
                controller.statusMonitor.isPerformAbility = true;
            }
            else
            {
                controller.StateMachine.TransitionToState(SlimeStateEnum.SlimeFollowState);
            }
        }
    }
}

