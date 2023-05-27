using System;
using UnityEngine;
using RPG.Ultilities;



namespace RPG.Character
{
    public class BossVisual : MonoBehaviour
    {
        #region Declarations
        private BossController bossController;
        private BossCombat bossCombat;
        private Animator animatorCmp;
        private int moveSpeedBlendHash;
        private int defaultAttackTriggerAnimHash;
        private int healingSpellTriggerAnimHash;
        private int summoningSpellTriggerAnimHash;
        private int fireBallSpellTriggerAnimHash;
        private int isDefeatedAnimHash;
        #endregion

        #region  Unity Methods

        private void Awake()
        {
            bossController = GetComponentInParent<BossController>();
            bossCombat = GetComponentInParent<BossCombat>();
            animatorCmp = GetComponent<Animator>();
            moveSpeedBlendHash = Helpers.StringToHash(GameConstants.MoveSpeedBlend);
            defaultAttackTriggerAnimHash = Helpers.StringToHash(GameConstants.DefaultAttackTriggerAnim);
            healingSpellTriggerAnimHash = Helpers.StringToHash(GameConstants.HealingSpellTriggerAnim);
            summoningSpellTriggerAnimHash = Helpers.StringToHash(GameConstants.SummoningSpellTriggerAnim);
            fireBallSpellTriggerAnimHash = Helpers.StringToHash(GameConstants.FireBallSpellTriggerAnim);
            isDefeatedAnimHash = Helpers.StringToHash(GameConstants.IsDefeatedBoolAnim);
        }

        private void Start()
        {
            BossEventManager.OnBossDefaultAttack += BossEventManager_OnBossDefaultAttack;
            BossEventManager.OnBossFireballSpell += BossEventManager_OnBossFireBallSpell;
            BossEventManager.OnBossSummoningSpell += BossEventManager_OnBossSummoningSpell;
            BossEventManager.OnBossHealingSpell += BossEventManager_OnBossHealingSpell;
            BossEventManager.OnBossDefeated += BossEventManager_OnBossDefeated;
            BossEventManager.OnBossMoveChanged += BossEventManager_OnBossMoveChanged;
        }


        private void OnDisable()
        {
            BossEventManager.OnBossDefaultAttack -= BossEventManager_OnBossDefaultAttack;
            BossEventManager.OnBossFireballSpell -= BossEventManager_OnBossFireBallSpell;
            BossEventManager.OnBossSummoningSpell -= BossEventManager_OnBossSummoningSpell;
            BossEventManager.OnBossHealingSpell -= BossEventManager_OnBossHealingSpell;
            BossEventManager.OnBossDefeated -= BossEventManager_OnBossDefeated;
            BossEventManager.OnBossMoveChanged -= BossEventManager_OnBossMoveChanged;
        }



        #endregion

        #region  Event Listeners
        private void BossEventManager_OnBossMoveChanged(object sender, BossEventManager.OnBossMoveChangedArgs e)
        {
            animatorCmp.SetFloat(moveSpeedBlendHash, e.moveSpeedBlend);
        }
        private void BossEventManager_OnBossDefaultAttack(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(defaultAttackTriggerAnimHash);
        }
        private void BossEventManager_OnBossSummoningSpell(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(summoningSpellTriggerAnimHash);
        }

        private void BossEventManager_OnBossHealingSpell(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(healingSpellTriggerAnimHash);
        }

        private void BossEventManager_OnBossFireBallSpell(object sender, EventArgs e)
        {
            animatorCmp.SetTrigger(fireBallSpellTriggerAnimHash);
        }


        private void BossEventManager_OnBossDefeated(object sender, EventArgs e)
        {
            animatorCmp.SetBool(isDefeatedAnimHash, true);
        }
        #endregion

        #region Animation Events

        //---Default Attack
        private void OnDefaultAttackLanded()
        {
            bossCombat.HandleAbilityDealDamage();
        }
        private void OnDefaultAttackEnd()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
        }
        //---Fireball Spell
        private void OnFireballSpellAffect()
        {
            bossCombat.HandleCastingFireball();
        }
        private void OnFireballSpellEnd()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
            StartCoroutine(bossCombat.CastFireballCooldown());
        }
        //---Summoning Spell
        private void OnSummoningSpellStart()
        {
            GlobalEventManager.OnBossSummonedEnemiesRaised(transform);
        }

        private void OnSummoningSpellAffect()
        {
            bossCombat.HandleSummonEnemy();
        }
        private void OnSummoningSpellEnd()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
            StartCoroutine(bossCombat.SummonEnemyCooldown());
        }
        //---Healing Spell
        private void OnHealingSpellAffect()
        {
        }


        private void OnHealingSpellEnd()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
        }
        private void OnDefeatedAnimEnd()
        {
            bossController.ActionsRecord.isPerformingAbility = false;
            bossController.ActionsRecord.isDefeated = true;
            bossController.HandleAfterDefeated();
        }
        private void CastingSpellAffect()
        {
        }

    }




    #endregion

}


