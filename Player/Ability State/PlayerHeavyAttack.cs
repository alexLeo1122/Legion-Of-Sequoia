using RPG.Character;
using UnityEngine;


namespace RPG.Ultilities
{
    [Ability(PlayerAbilityEnum.HeavyAttack)]

    public class PlayerHeavyAttack : PlayerBaseAbility
    {
        public PlayerHeavyAttack(PlayerAbilityEnum abilityEnum) : base(abilityEnum)
        {
        }
    }

}
