using System;
using RPG.Ultilities;
namespace RPG.Character
{
    public class AbilityAttribute : Attribute
    {
        public PlayerAbilityEnum abilityEnum;

        public AbilityAttribute(PlayerAbilityEnum abilityEnum)
        {
            this.abilityEnum = abilityEnum;
        }
    }
}

