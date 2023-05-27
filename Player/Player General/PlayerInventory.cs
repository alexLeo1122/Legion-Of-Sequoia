using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;
using System;

namespace RPG.Character
{
    public class PlayerInventory : MonoBehaviour
    {
        private Transform rightHandSlot;
        public Transform RightHandSlot { get => rightHandSlot; set => rightHandSlot = value; }
        private Transform leftHandSlot;
        public Transform LeftHandSlot { get => leftHandSlot; set => leftHandSlot = value; }
        public Weapon CurrentWeapon { get; set; }
        //public Weapon nearestWeaponInRange;
        private List<Weapon> WeaponInRangeList { get; set; } = new List<Weapon>();
        public bool IsHoldingWeapon { get { return CurrentWeapon != null; } }
        private PlayerController playerController;
        public Chest ChestCmp { get; private set; }

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }
        private void Start()
        {
            ChestCmp = GameObject.FindGameObjectWithTag(GameConstants.ChestTag).GetComponent<Chest>();
            Initialize();

        }
        public void EquipItem()
        {
            if (ChestCmp.GettingFirstWeaponQuest.isCompleted || !GetNearestWeaponInRange(out Weapon nearestWeapon)) return;
            if (CurrentWeapon != null)
            {
                Destroy(CurrentWeapon.gameObject);
            }
            RemoveFromWeaponInRangeList(nearestWeapon);
            GameObject newWeapon = Instantiate(nearestWeapon.WeaponStatSO.weaponPrefab, rightHandSlot);
            newWeapon.layer = LayerMask.NameToLayer(GameConstants.EquippedWeaponLayer);
            CurrentWeapon = newWeapon.GetComponent<Weapon>();
            if (ChestCmp.GettingFirstWeaponQuest.isCompleted == false)
            {
                ChestCmp.GettingFirstWeaponQuest.isCompleted = true;
            }
            Destroy(nearestWeapon.gameObject);
            CurrentWeapon.transform.localPosition = Vector3.zero;
            CurrentWeapon.transform.localRotation = Quaternion.Euler(Vector3.up * 90f);
            GlobalEventManager.OnPickingUpWeaponRaised();
        }
        //potion
        public void PickingUpPotion(GameObject potion)
        {
            var Potion = Instantiate(potion, leftHandSlot);
            Potion.transform.localPosition = Vector3.zero;
            Potion.transform.localRotation = Quaternion.identity;
        }

        public bool GetNearestWeaponInRange(out Weapon nearestWeapon)
        {
            nearestWeapon = null;
            if (WeaponInRangeList.Count == 0) return false;
            var instance = PlayerController.Instance;
            var nearestSquaredDistance = Mathf.Infinity;
            foreach (var item in WeaponInRangeList)
            {
                var itemSquaredDistance = (item.transform.position - instance.transform.position).sqrMagnitude;
                if (itemSquaredDistance < nearestSquaredDistance)
                {
                    nearestWeapon = item;
                    nearestSquaredDistance = itemSquaredDistance;
                }
            }

            return nearestWeapon != null;
        }


        public void AddToWeaponInRangeList(Weapon newWeapon)
        {
            if (WeaponInRangeList.Contains(newWeapon)) return;
            WeaponInRangeList.Add(newWeapon);

        }
        public void RemoveFromWeaponInRangeList(Weapon oldWeapon)
        {
            WeaponInRangeList.Remove(oldWeapon);
        }
        public void Initialize()
        {
            SetUpPickingUpCollider();
        }
        public void SetUpPickingUpCollider()
        {
            var pickingUpCollider = playerController.GetComponentInChildren<SphereCollider>();
            pickingUpCollider.radius = playerController.PlayerStatSO.pickUpRange;
        }
        public void RotateTowardNearestWeaponInRange(Weapon weapon)
        {
            Vector3 newDirectionVector = weapon.transform.position - playerController.transform.position;
            newDirectionVector.y = 0;
            playerController.SetDirectionVector(newDirectionVector);
        }
    }

}


