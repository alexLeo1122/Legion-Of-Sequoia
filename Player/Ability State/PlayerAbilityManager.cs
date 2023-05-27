using System.Collections;
using System.Collections.Generic;
using RPG.Ultilities;
using RPG.Character;
using System.Linq;
using System.Reflection;
using System;
using UnityEngine;

namespace RPG.Ultilities
{
    public class PlayerAbilityManager
    {
        public PlayerBaseAbility CurrentAbility { get; private set; }

        public Dictionary<PlayerAbilityEnum, PlayerBaseAbility> AbilityDict { get; private set; }

        public PlayerAbilityManager()
        {
            AbilityDict = new Dictionary<PlayerAbilityEnum, PlayerBaseAbility>();
            //Get all type of Player abilities
            var assembly = typeof(PlayerAbilityManager).Assembly;
            var abilityArray = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(PlayerBaseAbility)));
            foreach (var ability in abilityArray)
            {
                var attribute = ability.GetCustomAttribute<AbilityAttribute>();
                var tempEnum = attribute.abilityEnum;
                //Create ability instance based on tempEnum attribute
                var paramsArray = new object[] { tempEnum };
                ConstructorInfo constructor = ability.GetConstructor(new Type[] { typeof(PlayerAbilityEnum) });
                PlayerBaseAbility abilityObject = (PlayerBaseAbility)constructor.Invoke(new object[] { tempEnum });
                if (AbilityDict.ContainsKey(tempEnum)) continue;
                AbilityDict.Add(tempEnum, abilityObject);
            }

        }
        public void PerformAbility(PlayerAbilityEnum abilityEnum)
        {
            CurrentAbility = AbilityDict[abilityEnum];
            if (!CurrentAbility.isUnlocked) return;
            CurrentAbility.PerformAbility();
        }
        public void UnlockAbility(PlayerAbilityEnum abilityEnum)
        {
            var abilityToUnlock = AbilityDict[abilityEnum];
            abilityToUnlock.isUnlocked = true;
        }

    }
}
