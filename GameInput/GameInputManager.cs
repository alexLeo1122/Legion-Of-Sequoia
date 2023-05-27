using RPG.Character;
using UnityEngine;


namespace RPG.Core
{
    public class GameInputManager : MonoBehaviour
    {
        [SerializeField] GameInputSO gameInputSO;
        public GameInputSO GameInputSO => gameInputSO;
        private void Awake()
        {
            gameInputSO.Initialize();
        }
    }
}


