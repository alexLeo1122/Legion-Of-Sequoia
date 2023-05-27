
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    [BossAbility(BossAbilityEnum.FireBallSpell)]
    public class BossFireBallSpell : BossBaseAbility
    {
        public BossFireBallSpell(BossController controller, BossAbilityEnum abilityEnum) : base(controller, abilityEnum)
        {
        }
    }
}


