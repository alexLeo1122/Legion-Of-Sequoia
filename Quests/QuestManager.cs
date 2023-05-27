using RPG.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestSO[] QuestCollections;
        public struct QuestMonitor
        {
            public QuestSO CurrentQuest;
            public int currentQuestId;
        }
        public QuestMonitor questMonitor;
        public QuestSO CurrentQuest => questMonitor.CurrentQuest;
        private void Start()
        {
            Initialize();
        }
        private void Initialize()
        {
            foreach (var quest in QuestCollections)
            {
                quest.isComplete = false;
                quest.isActive = false;
            }

            questMonitor = new QuestMonitor() { currentQuestId = 0 };
            questMonitor.CurrentQuest = QuestCollections[questMonitor.currentQuestId];
        }
        public void TransitionToNextQuest()
        {
            if (questMonitor.currentQuestId == QuestCollections.Length - 1)
            {
                GlobalEventManager.OnGameEndRaised(true);
                return;
            } 
            questMonitor.currentQuestId++;
            questMonitor.CurrentQuest = QuestCollections[questMonitor.currentQuestId];
        }

    }
}

