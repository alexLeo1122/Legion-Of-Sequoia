using System;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyVisual : MonoBehaviour
    {
        #region Declarations

        [SerializeField] private EnemyController enemyController;
        private EnemyActionsSO enemyActionsSO;
        private Animator animatorCmp;
        private int moveSpeedBlendHash;
        private int defaultAttackTriggerAnimHash;
        private int heavyAttackTriggerAnimHash;
        private int isDefeatedAnimHash;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            animatorCmp = GetComponent<Animator>();
            moveSpeedBlendHash = Helpers.StringToHash(GameConstants.MoveSpeedBlend);
            defaultAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.DefaultAttackTriggerAnim);
            isDefeatedAnimHash = Helpers.StringToHash(GameConstants.IsDefeatedBoolAnim);
            heavyAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.HeavyAttackTriggerAnim);
            enemyActionsSO = enemyController.EnemyActionsSO;
            //Debug.Log("aake " + (enemyActionsSO== null));//this never works

        }
        /// <summary>
        /// this works for normal enemy
        /// </summary>
        private void Start()
        {
            if (enemyActionsSO == null)
            {
                enemyActionsSO = enemyController.EnemyActionsSO;
            }
            enemyActionsSO.OnEnemyDefaultAttack += EnemyActionsSO_OnEnemyDefaultAttack;
            enemyActionsSO.OnEnemyDefeated += EnemyActionsSO_OnEnemyDefeated;
            enemyActionsSO.OnEnemyMoveChanged += EnemyActionsSO_OnEnemyMoveChanged;
            enemyActionsSO.OnEnemyHeavyAttack += EnemyActionsSO_OnEnemyHeavyAttack;
        }
        private void OnEnable()
        {
            if (!enemyController.IsSummonedEnemy) return;
            if (enemyActionsSO == null)
            {
                enemyActionsSO = enemyController.EnemyActionsSO;
            }
            enemyActionsSO.OnEnemyDefaultAttack += EnemyActionsSO_OnEnemyDefaultAttack;
            enemyActionsSO.OnEnemyDefeated += EnemyActionsSO_OnEnemyDefeated;
            enemyActionsSO.OnEnemyMoveChanged += EnemyActionsSO_OnEnemyMoveChanged;
            enemyActionsSO.OnEnemyHeavyAttack += EnemyActionsSO_OnEnemyHeavyAttack;
        }

        private void OnDisable()
        {
            enemyActionsSO.OnEnemyDefaultAttack -= EnemyActionsSO_OnEnemyDefaultAttack;
            enemyActionsSO.OnEnemyDefeated -= EnemyActionsSO_OnEnemyDefeated;
            enemyActionsSO.OnEnemyMoveChanged -= EnemyActionsSO_OnEnemyMoveChanged;
            enemyActionsSO.OnEnemyHeavyAttack -= EnemyActionsSO_OnEnemyHeavyAttack;
        }



        private void EnemyActionsSO_OnEnemyMoveChanged(object sender, EnemyActionsSO.OnEnemyMoveChangedArgs e)
        {
            animatorCmp.SetFloat(moveSpeedBlendHash, e.moveSpeedBlend);
        }


        #endregion

        #region Event Listeners
        private void EnemyActionsSO_OnEnemyIdle(object sender, EventArgs e)
        {
            animatorCmp.SetFloat(moveSpeedBlendHash, 0f);
        }
        private void EnemyActionsSO_OnEnemyDefaultAttack(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(defaultAttackTriggerAnimHash);
        }
        private void EnemyActionsSO_OnEnemyHeavyAttack(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(heavyAttackTriggerAnimHash);
        }
        private void EnemyActionsSO_OnEnemyDefeated(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(isDefeatedAnimHash);
        }
        #endregion

        #region Animation Event
        ////Default Attack
        private void OnDefaultAttackLanded()
        {
            enemyController.CombatCmp.HandleAbilityDealDamage();
        }
        private void OnDefaultAttackEnd()
        {
            enemyController.ActionsRecord.isAttacking = false;
            //enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyIdleState);
        }
        private void OnHeavyAttackLanded()
        {

        }
        private void OnHeavyAttackEnd()
        {
            enemyController.ActionsRecord.isAttacking = false;
        }
        private void OnDefeatedAnimEnd()
        {
            enemyController.ActionsRecord.isDefeated = true;
            enemyController.HandleAfterDefeated();
        }
        #endregion
    }
}

