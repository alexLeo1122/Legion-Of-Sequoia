using RPG.Character;
using UnityEngine;

namespace RPG.Ultilities
{
    [Ability(PlayerAbilityEnum.DashFrontAttack)]
    public class PlayerDashFrontAttack : PlayerBaseAbility
    {
        public PlayerDashFrontAttack(PlayerAbilityEnum abilityEnumArgs) : base(abilityEnumArgs)
        {
        }
    }
}



