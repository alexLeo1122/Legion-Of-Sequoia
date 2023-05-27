
using UnityEngine;


namespace RPG.Character
{
    public class EnemyCombat : CharacterCombat
    {
        private EnemyController enemyController;
        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
        }
        public void DefaultAttack()
        {
            DamageToDeal = enemyController.EnemyStatSO.defaultAttackDamage;
            enemyController.EnemyActionsSO.OnEnemyDefaultAttackRaised();
        }
        public override void HandleAbilityDealDamage()
        {
            PlayerController.Instance.HealthCmp.TakeDamage(DamageToDeal);
        }

        public override void HandleCastingSpellAffectTarget()
        {
            //Future Extension
        }
    }
}
