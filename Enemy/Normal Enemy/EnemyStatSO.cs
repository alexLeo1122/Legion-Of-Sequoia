
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(fileName = "EnemyStatSO", menuName = "ScriptableObjects/EnemyStatSO")]

    public class EnemyStatSO : CharacterStatSO
    {
        public float chaseRange = 1.8f;
        public float runSpeed;
        public float patrolSpeed;
    }

}

