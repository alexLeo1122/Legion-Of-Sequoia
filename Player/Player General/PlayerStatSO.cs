
using RPG.Ultilities;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "PlayerStatSO", menuName = "ScriptableObjects/PlayerStatSO")]
    public class PlayerStatSO : CharacterStatSO
    {
        //Ability
        [Ability(PlayerAbilityEnum.PickingUp)]
        public float pickUpRange = 0.6f;
        [Ability(PlayerAbilityEnum.HeavyAttack)]
        public float heavyAttackDamage;
        [Ability(PlayerAbilityEnum.DashFrontAttack)]
        public float dashFrontAttackDamage;
        [Ability(PlayerAbilityEnum.HealingSpell)]
        public float healingSpellAmount;

        
    }
}

