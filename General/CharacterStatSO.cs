
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class CharacterStatSO : ScriptableObject
    {
        public float defaultHealth;
        public float maxHealth;
        public float defaultSpeed;
        public float maxSpeed;
        //Attribute
        [Ability(PlayerAbilityEnum.DefaultAttack), BossAbility(BossAbilityEnum.DefaultAttack)]
        public float defaultAttackDamage;

        public float attackRange = 0.6f;
    }
}


