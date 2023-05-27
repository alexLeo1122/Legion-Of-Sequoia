
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    [BossAbility(BossAbilityEnum.DefaultAttack)]
    public class BossDefaultAttack : BossBaseAbility
    {
        public BossDefaultAttack(BossController controller, BossAbilityEnum abilityEnum) : base(controller, abilityEnum)
        {
        }
    }
}
