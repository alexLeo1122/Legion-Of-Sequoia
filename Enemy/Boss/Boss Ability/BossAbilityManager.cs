using System.Collections;
using System.Collections.Generic;
using RPG.Ultilities;
using System.Linq;
using System.Reflection;
using System;
using UnityEngine;
using RPG.Character;

namespace RPG.Character
{
    public class BossAbilityManager
    {
        private BossController bossController;
        public BossBaseAbility CurrentAbility { get; private set; }
        public Dictionary<BossAbilityEnum, BossBaseAbility> AbilityDict { get; private set; }

        public BossAbilityManager(BossController bossController)
        {
            this.bossController = bossController;

            AbilityDict = new Dictionary<BossAbilityEnum, BossBaseAbility>();
            //Get all type of Player abilities
            var assembly = typeof(BossAbilityManager).Assembly;
            var abilityArray = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(BossBaseAbility)));
            foreach (var ability in abilityArray)
            {
                var attribute = ability.GetCustomAttribute<BossAbilityAttribute>();
                var tempEnum = attribute.abilityEnum;
                //Create ability instance based on tempEnum attribute
                var paramsArray = new object[] { bossController, tempEnum };
                ConstructorInfo constructor = ability.GetConstructor(new Type[] { typeof(BossController), typeof(BossAbilityEnum) });
                BossBaseAbility abilityObject = (BossBaseAbility)constructor.Invoke(new object[] { bossController, tempEnum });
                if (AbilityDict.ContainsKey(tempEnum)) continue;
                AbilityDict.Add(tempEnum, abilityObject);
            }
        }
        public void PerformAbility(BossAbilityEnum abilityEnum)
        {
            CurrentAbility = AbilityDict[abilityEnum];
            CurrentAbility.PerformAbility();
        }



    }
}
















