using RPG.Ultilities;
using System;


namespace RPG.Character
{
    public class TypeAbilityAttribute : Attribute
    {
        public PlayerTypeAbilityEnum typeAbilityEnum;
        public TypeAbilityAttribute (PlayerTypeAbilityEnum enumArgs)
        {
            typeAbilityEnum = enumArgs;
        }
    }
}


