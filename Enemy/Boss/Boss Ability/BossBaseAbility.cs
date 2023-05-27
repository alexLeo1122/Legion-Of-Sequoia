using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class BossBaseAbility : CharacterBaseAbility
    {
        protected BossController bossController;
        protected float damageToSet = 0f;
        public BossStatSO StatSO => bossController.BossStatSO;
        public MethodInfo StoredMethod { get; protected set; }
        public BossBaseAbility(BossController controller, BossAbilityEnum abilityEnumArgs)
        {
            bossController = controller;
            var statSOType = StatSO.GetType();
            var fieldArray = statSOType.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fieldInfo => fieldInfo.GetCustomAttribute<BossAbilityAttribute>() != null).ToArray();
            foreach (var fieldInfo in fieldArray)
            {
                BossAbilityAttribute attribute = fieldInfo.GetCustomAttribute<BossAbilityAttribute>();
                if (attribute.abilityEnum == abilityEnumArgs)
                {
                    damageToSet = (float)fieldInfo.GetValue(StatSO);
                    break;
                }
            }
            Type actionType = typeof(BossEventManager);
            var MethodArray = actionType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(MethodInfo => MethodInfo.GetCustomAttribute<BossAbilityAttribute>() != null).ToArray();
            //Debug.Log("Length: " + MethodArray.Length);
            foreach (var methodInfo in MethodArray)
            {
                BossAbilityAttribute attribute = methodInfo.GetCustomAttribute<BossAbilityAttribute>();
                if (attribute.abilityEnum == abilityEnumArgs)
                {
                    StoredMethod = methodInfo;
                    //Debug.Log(StoredMethod);
                    break;
                }
            }
        }

        public override void PerformAbility()
        {
            bossController.CombatCmp.SetAttackDamage(damageToSet);
            StoredMethod.Invoke(null, null);
        }

    }



}

