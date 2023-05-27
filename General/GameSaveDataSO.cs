
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "GameDataSO", menuName = "ScriptableObjects/GameDataSO")]
    public class GameSaveDataSO : ScriptableObject
    {
        public GameObject[] heroPrefab;
        public int choiceIndex;
        public int HeroChoice => choiceIndex;
        public void SetHeroChoice(int choice)
        {
            choiceIndex = choice;
        }
    }
}



