using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.Character
{
    public class BossCombat : CharacterCombat
    {
        #region Declarations
        [SerializeField] private Transform firePoint;
        public Transform FirePoint => firePoint;
        [SerializeField] private int maxSummonedEnemies = 1;
        private BossController bossController;
        public bool IsCastFireballCoolDown { get; set; } = false;
        public float FireballCoolDownTime { get; set; } = 1f;

        //set up cooldown for SummonEnemies
        public bool IsSummonEnemyCoolDown { get; set; } = false;
        public float SummonEnemyCoolDownTime { get; set; } = 4f;


        #endregion

        #region Unity Methods
        private void Awake()
        {
            bossController = GetComponent<BossController>();
            BossEventManager.OnProjectileHitTarget += BossEventManager_OnProjectileHitTarget;
            BossEventManager.OnSettingProjectileData += BossEventManager_OnGettingProjectileData;
        }

        private void OnDisable()
        {
            BossEventManager.OnProjectileHitTarget -= BossEventManager_OnProjectileHitTarget;
            BossEventManager.OnSettingProjectileData -= BossEventManager_OnGettingProjectileData;
        }

        private void BossEventManager_OnGettingProjectileData(object sender, BossEventManager.OnSettingProjectileDataArgs e)
        {
            var fireBall = e.fireball;
            fireBall.transform.position = firePoint.position;
            fireBall.FireballDirection = bossController.GetDirectionTowardsPlayer();
            fireBall.transform.rotation = Quaternion.LookRotation(bossController.GetDirectionTowardsPlayer());
        }

        #endregion

        #region Event Handlers
        private void BossEventManager_OnProjectileHitTarget(object sender, System.EventArgs e)
        {
            HandleAbilityDealDamage();
        }
        #endregion

        #region Inherited Methods
        public override void HandleAbilityDealDamage()
        {
            PlayerController.Instance.HealthCmp.TakeDamage(DamageToDeal);
        }

        public override void HandleCastingSpellAffectTarget()
        {
        }

        //Actually casting a fireball
        public void HandleCastingFireball()
        {
            var fireball = bossController.ProjectilePool.GetCurrentPool().Get();
        }
        public IEnumerator CastFireballCooldown()
        {
            IsCastFireballCoolDown = true;
            yield return new WaitForSeconds(FireballCoolDownTime);
            IsCastFireballCoolDown = false;
        }
        public IEnumerator SummonEnemyCooldown()
        {
            IsSummonEnemyCoolDown = true;
            yield return new WaitForSeconds(SummonEnemyCoolDownTime);
            IsSummonEnemyCoolDown = false;
        }
        public void HandleSummonEnemy()
        {
            if (bossController.EnemyPool.ActiveSummonEnemiesList.Count >= maxSummonedEnemies) return;
            bossController.EnemyPool.SummonEnemyInSpawnLocations();
        }
        public bool IsSummonEnemiesReachMax => bossController.EnemyPool.ActiveSummonEnemiesList.Count == maxSummonedEnemies;

        #endregion
    }

}


