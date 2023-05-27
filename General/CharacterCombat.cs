
using RPG.Ultilities;
using UnityEngine;


namespace RPG.Character
{
    public abstract class CharacterCombat : MonoBehaviour
    {
        public float DamageToDeal { get; protected set; }
        public PlayerAbilityEnum activeAbility { get; set; }
        public virtual void DealDamage(CharacterHealth healthCmp)
        {
            healthCmp.TakeDamage(DamageToDeal);
        }
        public void SetAttackDamage(float damageToUpdate)
        {
            DamageToDeal = damageToUpdate;
        }

        public abstract void HandleAbilityDealDamage();
        public abstract void HandleCastingSpellAffectTarget();

    }

}

