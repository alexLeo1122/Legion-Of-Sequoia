using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "BossStatSO", menuName = "ScriptableObjects/BossStatSO")]

    public class BossStatSO : EnemyStatSO
    {
        //Ability
        [BossAbility(BossAbilityEnum.SummoningSpell)]
        public float summoningRange = 1.5f;
        [BossAbility(BossAbilityEnum.FireBallSpell)]
        public float fireBallSpellDamage = 20f;
        public float spellCastingRange = 2f;
        public float skillCoolDownTime = 2.5f;
        [BossAbility(BossAbilityEnum.HealingSpell)]
        public float healingAmout = 40f;
    }
}




