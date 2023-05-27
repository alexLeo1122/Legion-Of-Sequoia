using UnityEngine;

namespace RPG.Character
{
    public class BossMovement : CharacterMovement
    {
        [SerializeField] private Transform teleportPos;
        public Transform TeleportPos => teleportPos;
        public override void MoveAgent(Vector3? direction = null)
        {
            if (direction.HasValue)
            {
                Vector3 directionVector = direction.Value;
                Agent.SetDestination(directionVector);
            }
        }
        public void StopAgent()
        {
            if (!Agent.hasPath) return;
            Agent.ResetPath();
        }

        public void SetAgentSpeed(float newSpeed)
        {
            Agent.speed = newSpeed;
        }
    }

}

