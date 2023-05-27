using System;

using RPG.Ultilities;
using UnityEngine;

namespace RPG.Character
{
    public static class BossEventManager
    {
        #region Events 

        //---Boss State Events + Boss Movevent Events
        public static event EventHandler<OnBossAppearedArgs> OnBossAppeared;
        public static event EventHandler OnBossIdle;
        public static event EventHandler OnBossReturning;
        public static event EventHandler OnBossChase;
        public static event EventHandler<OnBossMoveChangedArgs> OnBossMoveChanged;



        //---Perform Ability Events
        public static event EventHandler OnBossDefaultAttack;
        public static event EventHandler OnBossFireballSpell;
        public static event EventHandler OnBossSummoningSpell;
        public static event EventHandler OnBossHealingSpell;
        public static event EventHandler OnBossDefeated;
        //Boss casting a projectile, actually
        public static event EventHandler OnBossCastingFireball;
        public static event EventHandler OnProjectileHitTarget;
        public static event EventHandler<OnReturnProjectileToPoolArgs> OnReturnProjectileToPool;
        public static event EventHandler<OnSettingProjectileDataArgs> OnSettingProjectileData;
        public static event EventHandler<OnReturnSummonedEnemyToPoolArgs> OnReturnSummonedEnemyToPool;
        //---End of Perform Ability Events
        #endregion

        #region Event Args

        public class OnBossAppearedArgs : EventArgs
        {
            public CharacterHealth healthCmp;
        }
        public class OnBossMoveChangedArgs : EventArgs
        {
            public float moveSpeedBlend;
        }
        public class OnSettingProjectileDataArgs : EventArgs
        {
            public Fireball fireball;
        }
        public class OnReturnProjectileToPoolArgs : EventArgs
        {
            public Fireball fireball;
        }
        public class OnReturnSummonedEnemyToPoolArgs : EventArgs
        {
            public GameObject summonedEnemy;
        }
        #endregion


        #region Raised Event Method
        //---Boss State Events + Boss Movevent
        public static void OnBossAppearedRaised(CharacterHealth healthCmp)
        {
            OnBossAppeared?.Invoke(null, new OnBossAppearedArgs { healthCmp = healthCmp });
        }
        public static void OnBossIdleRaised()
        {
            OnBossIdle?.Invoke(null, EventArgs.Empty);
        }
        public static void OnBossReturningRaised()
        {
            OnBossReturning?.Invoke(null, EventArgs.Empty);
        }
        public static void OnBossMoveChangedRaised(object sender, OnBossMoveChangedArgs args)
        {
            OnBossMoveChanged?.Invoke(sender, args);
        }
        public static void OnBossChaseRaised()
        {
            OnBossChase?.Invoke(null, EventArgs.Empty);
        }

        //---Default Attack
        [BossAbility(BossAbilityEnum.DefaultAttack)]
        public static void OnBossDefaultAttackRaised()
        {
            OnBossDefaultAttack?.Invoke(null, EventArgs.Empty);
        }
        //---Healing Spell
        [BossAbility(BossAbilityEnum.HealingSpell)]
        public static void OnBossHealingSpellRaised()
        {
            OnBossHealingSpell?.Invoke(null, EventArgs.Empty);
        }
        //---Fire Ball Spell
        [BossAbility(BossAbilityEnum.FireBallSpell)]
        public static void OnBossFireBallSpellRaised()
        {
            OnBossFireballSpell?.Invoke(null, EventArgs.Empty);
        }
        //---Summoning Spell
        [BossAbility(BossAbilityEnum.SummoningSpell)]
        public static void OnBossSummoningSpellRaised()
        {
            OnBossSummoningSpell?.Invoke(null, EventArgs.Empty);
        }
        //---Return Summoned Enemy To Pool
        public static void OnReturnSummonedEnemyToPoolRaised(GameObject sender, OnReturnSummonedEnemyToPoolArgs args)
        {
            OnReturnSummonedEnemyToPool?.Invoke(sender, args);
        }
        //---Defeated
        public static void OnBossDefeatedRaised()
        {
            OnBossDefeated?.Invoke(null, EventArgs.Empty);
        }


        //---Casting Fireball
        public static void OnBossCastingFireballRaised()
        {
            OnBossCastingFireball?.Invoke(null, EventArgs.Empty);
        }

        public static void OnSettingProjectileDataRaised(GameObject sender, OnSettingProjectileDataArgs args)
        {
            OnSettingProjectileData?.Invoke(sender, args);
        }

        //---Projectile Hit Target
        public static void OnProjectileHitTargetRaised(GameObject target)
        {
            OnProjectileHitTarget?.Invoke(null, EventArgs.Empty);
        }
        //---Return Projectile To Pool
        public static void OnReturnProjectileToPoolRaised(GameObject sender, OnReturnProjectileToPoolArgs args)
        {
            OnReturnProjectileToPool?.Invoke(sender, args);
        }

        #endregion

    }

}

