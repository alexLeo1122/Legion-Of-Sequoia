
using UnityEngine;

namespace RPG.Core
{
    public class GameSaveManager : MonoBehaviour
    {
        [SerializeField] private GameSaveDataSO heroChoiceSO;
        public GameSaveDataSO HeroChoice => heroChoiceSO;
    }
}


