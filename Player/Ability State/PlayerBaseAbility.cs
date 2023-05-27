using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public abstract class PlayerBaseAbility : CharacterBaseAbility
    {
        protected PlayerController playerController = PlayerController.Instance;
        protected float damageToSet = 0f;
        public bool isUnlocked = false;
        public PlayerActionsSO ActionsSO => playerController.PlayerActionsSO;
        public PlayerStatSO StatSO => playerController.PlayerStatSO;
        public MethodInfo StoredMethod { get; protected set; }
        public PlayerBaseAbility(PlayerAbilityEnum abilityEnumArgs)
        {
            //Getting Damage of PlayerabilityEnum from PlayerStatSO
            Type statSOType = StatSO.GetType();
            var fieldArray = statSOType.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fieldInfo => fieldInfo.GetCustomAttribute<AbilityAttribute>() != null).ToArray();
            foreach ( var fieldInfo in fieldArray )
            {
                AbilityAttribute attribute = fieldInfo.GetCustomAttribute<AbilityAttribute>();
                if (attribute.abilityEnum == abilityEnumArgs)
                {
                    damageToSet = (float) fieldInfo.GetValue(StatSO);
                    break;
                }
            }

            Type actionSOType = ActionsSO.GetType();
            var MethodArray = actionSOType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(MethodInfo => MethodInfo.GetCustomAttribute<AbilityAttribute>() != null).ToArray();
            foreach (var methodInfo in MethodArray)
            {
                AbilityAttribute attribute = methodInfo.GetCustomAttribute<AbilityAttribute>();
                if (attribute.abilityEnum == abilityEnumArgs)
                {
                    StoredMethod = methodInfo;
                    break;
                }
            }
        }

        public override void PerformAbility()
        {
            playerController.CombatCmp.SetAttackDamage(damageToSet);
            StoredMethod.Invoke(ActionsSO, null);
        }

    }


}

