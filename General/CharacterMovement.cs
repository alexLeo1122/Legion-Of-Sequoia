
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Character
{
    public abstract class CharacterMovement : MonoBehaviour
    {
        public NavMeshAgent Agent { get; private set; }

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        public abstract void MoveAgent(Vector3? direction = null);

    }

}

