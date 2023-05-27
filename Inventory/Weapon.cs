using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Character
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private WeaponStatSO weaponStatSO;
        public WeaponStatSO WeaponStatSO => weaponStatSO;

        public event EventHandler OnWeaponPickedUp;

        public void OnWeaponPickedUpRaised()
        {
            OnWeaponPickedUp?.Invoke(this, EventArgs.Empty);
        }

    }
}

