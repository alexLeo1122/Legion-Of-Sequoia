using UnityEngine;
using RPG.Core;

namespace RPG.Character
{
    public class HeroChoice : MonoBehaviour
    {
        [SerializeField] private GameSaveManager gameSaveManager;
        [SerializeField] private GameObject visualIndex0;
        [SerializeField] private GameObject visualIndex1;
        [SerializeField] private GameObject visualIndex2;

        private void Awake()
        {
            if (gameSaveManager.HeroChoice.choiceIndex == 0)
            {
                visualIndex0.SetActive(true);
            }
            else if (gameSaveManager.HeroChoice.choiceIndex == 1)
            {
                visualIndex1.SetActive(true);
            }
            else
            {
                visualIndex2.SetActive(true);
            }
        }

    }
}


