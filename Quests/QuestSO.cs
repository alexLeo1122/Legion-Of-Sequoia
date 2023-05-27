using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{ 
    [CreateAssetMenu(fileName = "QuestSO", menuName = "ScriptableObjects/Quests/QuestSO")]
    public class QuestSO : ScriptableObject
    {
        public int id;
        public string questName;
        public string questDescription;
        public string reward;
        public TextAsset questStory;
        public bool isMainQuest;
        public bool isComplete;
        public bool isActive;
    }
}

