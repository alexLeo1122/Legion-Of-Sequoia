
using RPG.Allies;
using UnityEngine;
using UnityEngine.AI;
using RPG.Ultilities;

namespace RPG.Character
{
    public class SlimeVisual : MonoBehaviour
    {
        //private GameObject attackEffect;
        [SerializeField] private GameObject attackEffect;
        private SlimeController slimeController;
        private Animator animatorCmp;
        private NavMeshAgent agent;
        private float speedBlend => PlayerController.Instance.GetSpeedBlend();
        private PlayerController playerController => PlayerController.Instance;
        private void Awake()
        {
            slimeController = GetComponentInParent<SlimeController>();
            animatorCmp = GetComponent<Animator>();
            agent = GetComponentInParent<NavMeshAgent>();
        }
        private void OnEnable()
        {
            GlobalEventManager.OnSlimePerformAbility += GlobalEventManager_OnSlimePerformAbility;
        }
        private void OnDisable()
        {
            GlobalEventManager.OnSlimePerformAbility -= GlobalEventManager_OnSlimePerformAbility;
        }

        private void GlobalEventManager_OnSlimePerformAbility(object sender, GlobalEventManager.OnSlimePerformAblilityArgs e)
        {
            animatorCmp.SetTrigger("attack");
            attackEffect.SetActive(true);
        }

        private void OnSlimeAttackLanded ()
        {
            var target = playerController.CombatCmp.TargetEnemy;
            if (target == null ) return;
            if (!target.gameObject.CompareTag("Enemy")) return;
            target.TakeDamage(slimeController.DefaultDamage);
        }
        private void OnSlimeAttackAnimEnd()
        {
            attackEffect.SetActive(false);
            slimeController.statusMonitor.isPerformAbility = false;
            if (PlayerController.Instance.StateMachine.CurrentState is not PlayerPerformAbilityState)
            {
                slimeController.StateMachine.TransitionToState(SlimeStateEnum.SlimeFollowState);
            };
        }

    }

}

