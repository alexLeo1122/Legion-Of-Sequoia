
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    [BossAbility(BossAbilityEnum.SummoningSpell)]
    public class BossSummoningSpell : BossBaseAbility
    {
        public BossSummoningSpell(BossController controller, BossAbilityEnum abilityEnum) : base(controller, abilityEnum)
        {
        }
    }
}

