
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "WeaponStatSO", menuName = "ScriptableObjects/WeaponStatSO")]

    public class WeaponStatSO : ScriptableObject
    {
        public GameObject weaponPrefab;
        public float damage = 10f;
        public float attackRange = 1f;
        public float speed = 1f;

    }

}

