
using System;
using UnityEngine;
using RPG.Ultilities;


namespace RPG.Character
{
    public class ChestVisual : MonoBehaviour
    {
        #region Declarations
        [SerializeField] private Chest chest;
        private Animator animatorCmp;
        private int IsOpenBoolAnimHash;
        #endregion

        #region Unity Methods
        private void Start()
        {
            animatorCmp = GetComponent<Animator>();
            IsOpenBoolAnimHash = Helpers.StringToHash(GameConstants.IsOpenBoolAnim);
        }

        private void OnEnable()
        {
            GlobalEventManager.OnWeaponChestOpen += Chest_OnWeaponChestOpened;
        }
        private void OnDisable()
        {
            GlobalEventManager.OnWeaponChestOpen -= Chest_OnWeaponChestOpened;
        }
        #endregion

        #region Animation Events
        private void OnTresuareChestOpenAffect()
        {
            chest.RewardSpawnManager.SpawnRewards(chest.RewardWeaponArray);
        }
        private void OnTresuareChestOpenEnd()
        {
            Destroy(PlayerController.Instance.InventoryCmp.CurrentWeapon.gameObject);
        }
        #endregion

        #region Event Handlers
        private void Chest_OnWeaponChestOpened(object sender, EventArgs e)
        {
            animatorCmp.SetBool(IsOpenBoolAnimHash, true);
        }
        #endregion

    }

}

