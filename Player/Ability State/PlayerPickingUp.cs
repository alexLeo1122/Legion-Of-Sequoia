


using UnityEngine;
using RPG.Ultilities;



namespace RPG.Character
{
    [Ability(PlayerAbilityEnum.PickingUp)]

    public class PlayerPickingUp : PlayerBaseAbility
    {
        
        public PlayerPickingUp(PlayerAbilityEnum abilityEnum) : base(abilityEnum)
        {
        }
        
        public override void PerformAbility()
        {
            // rotate toward target to pick up
            //Rotate toward nearest weapon // this will be refactor for picking up more items
            if (playerController.InventoryCmp.GetNearestWeaponInRange(out Weapon nearestWeapon) &&
                playerController.InventoryCmp.CurrentWeapon == null
                )
            {
                playerController.InventoryCmp.RotateTowardNearestWeaponInRange(nearestWeapon);
                //need to refactor to QuestManager
            };

            StoredMethod.Invoke(ActionsSO, null);
        }
    }

}

