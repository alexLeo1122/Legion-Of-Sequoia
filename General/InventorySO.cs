
using UnityEngine;


namespace RPG.Character
{
    [CreateAssetMenu(fileName = "InventorySO", menuName = "ScriptableObjects/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        public GameObject SwordPrefab;
        public GameObject AxePrefab;
        public GameObject PotionPrefab;
    }
}

