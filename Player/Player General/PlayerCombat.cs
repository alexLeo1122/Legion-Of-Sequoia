

using UnityEngine;
using UnityEngine.AI;
using RPG.Ultilities;
using System.Collections.Generic;
using System;

namespace RPG.Character
{
    public class PlayerCombat : CharacterCombat
    {
        #region  Declarations
        public CharacterHealth TargetEnemy { get; set; }
        private PlayerController playerController;
        public struct EnemyMonitor
        {
            public List<CharacterHealth> healthList;
            public bool isNeareastEnemyInRange;
        }


        public EnemyMonitor enemyMonitor { get; private set; }

        #endregion

        #region Unity Methods
        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
            CharacterHealth.OnHealthComponentAppeared += EnemyController_OnEnemyAppearedNew;
        }

        private void EnemyController_OnEnemyAppearedNew(object sender, CharacterHealth.OnHealthComponentAppearedArgs e)
        {
            if (e.healthCmp.gameObject.CompareTag(GameConstants.PlayerTag)) return;
            if (enemyMonitor.healthList.Contains(e.healthCmp)) return;
            enemyMonitor.healthList.Add(e.healthCmp);
        }

        private void OnEnable()
        {
            enemyMonitor = new EnemyMonitor() {  healthList = new List<CharacterHealth>(), isNeareastEnemyInRange = false };
        }

        #endregion

        #region Inherited Methods
        public override void HandleAbilityDealDamage()
        {
            if (TargetEnemy == null) return;
            TargetEnemy.TakeDamage(DamageToDeal);
        }
        public override void HandleCastingSpellAffectTarget()
        {
            //Spell affect extension
            if (playerController.PlayerAbilityManager.CurrentAbility is PlayerHealingSpell)
            {
                playerController.HealthCmp.Heal(playerController.PlayerStatSO.healingSpellAmount);
            }
        }

        #endregion


        #region Custom Methods

        public bool IsNeareastEnemyInRange(out CharacterHealth targetHealthCmp)
        {
            bool isEnemyInrange = false;
            targetHealthCmp = null;
            if (enemyMonitor.healthList.Count == 0) return isEnemyInrange;
            //Check if neareast enemy is in range
            if (GetNearestEnemy().distance <= PlayerController.Instance.PlayerStatSO.attackRange)
            {
                targetHealthCmp = GetNearestEnemy().targetHealthCmp;
                isEnemyInrange = true;
                return isEnemyInrange;
            }
            return isEnemyInrange;
        }


        #endregion

        #region Event Handlers
        public (CharacterHealth targetHealthCmp, float distance) GetNearestEnemy()
        {
            if (enemyMonitor.healthList.Count == 0) return (null, Mathf.Infinity);
            CharacterHealth nearestEnemy = null;
            float minDistance = Mathf.Infinity;
            foreach (CharacterHealth enemy in enemyMonitor.healthList)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            return (nearestEnemy, minDistance);
        }



        #endregion
    }

}


