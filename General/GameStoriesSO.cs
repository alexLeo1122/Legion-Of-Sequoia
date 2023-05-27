using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "GameStoriesSO", menuName = "ScriptableObjects/GameStoriesSO")]
    public class GameStoriesSO : ScriptableObject
    {
        public TextAsset NPCWelcome;
        public TextAsset NPCSlimeQuest;
        public TextAsset NPCGettingFirstWeaponQuest;
    }

}



