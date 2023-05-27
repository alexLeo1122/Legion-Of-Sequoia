using RPG.Ultilities;
using System;



namespace RPG.Character
{
    public class BossAbilityAttribute : Attribute
    {
        public BossAbilityEnum abilityEnum;
        public BossAbilityAttribute(BossAbilityEnum enumArgs)
        {
            abilityEnum = enumArgs;
        }
    }

}
