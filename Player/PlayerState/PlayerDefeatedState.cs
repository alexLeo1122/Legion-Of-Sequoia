
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public class PlayerDefeatedState : PlayerBaseState
    {
        public PlayerDefeatedState(PlayerController controller) : base(controller)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            playerController.GameInputSO.CleanUp();
        }
    }
}

