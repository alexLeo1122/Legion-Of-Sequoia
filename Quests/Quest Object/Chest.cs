using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Ultilities;
using System;

namespace RPG.Character
{
    public class Chest : MonoBehaviour
    {

        #region Declarations
        [SerializeField] private RewardSpawnManager rewardSpawnManager;
        public RewardSpawnManager RewardSpawnManager => rewardSpawnManager;
        public WeaponStatSO[] RewardWeaponArray { get; private set; }
        private int numberOfWeaponToReward = 3;
        public QuestTest GettingFirstWeaponQuest;
        #endregion

        #region Unity Methods
        private void Start()
        {
            Initialize();
        }

        #endregion

        #region Custom Methods

        public void OnPlayerInteract()
        {
            GlobalEventManager.OnWeaponChestOpenRaised();
        }

        public void Initialize()
        {
            var allWeapons = Resources.LoadAll<WeaponStatSO>("Weapon");
            RewardWeaponArray = allWeapons.Shuffle().Take(numberOfWeaponToReward).ToArray();
            //this will be refactored using quest manager;
            GettingFirstWeaponQuest = new QuestTest()
            {
                id = 1,
                name = "Get first weapon",
                description = "Find a Tresuare Chest to get a weapon",
                isCompleted = false
            };
        }

 


        #endregion


    }

}

