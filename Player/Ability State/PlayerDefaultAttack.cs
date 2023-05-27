using RPG.Ultilities;



namespace RPG.Character
{
    //Attribule here
    [Ability(PlayerAbilityEnum.DefaultAttack)]

    public class PlayerDefaultAttack : PlayerBaseAbility
    {
        public PlayerDefaultAttack(PlayerAbilityEnum abilityEnum):base(abilityEnum)
        {
        }

    }
}




