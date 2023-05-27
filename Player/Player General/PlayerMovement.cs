
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Character {
    public class PlayerMovement : CharacterMovement
    {
        public override void MoveAgent(Vector3? direction = null)
        {
            Vector3 offset = PlayerController.Instance.MovementVector;
            Agent.Move(offset* Time.deltaTime* Agent.speed);
            GlobalEventManager.OnFollowPlayerMovedRaised(
                this, new GlobalEventManager.OnFollowPlayerMoveArgs() { movementVector = offset, playerSpeed = Agent.speed}
            );
        }
    }
}

