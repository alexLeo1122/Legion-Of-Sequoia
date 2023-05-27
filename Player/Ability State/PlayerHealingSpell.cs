using UnityEngine;
using RPG.Ultilities;   


namespace RPG.Character
{
    [Ability(PlayerAbilityEnum.HealingSpell)]
    public class PlayerHealingSpell : PlayerBaseAbility
    {
        public PlayerHealingSpell(PlayerAbilityEnum abilityEnum) : base(abilityEnum)
        {
        }

    }
}


