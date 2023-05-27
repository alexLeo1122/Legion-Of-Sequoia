using RPG.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;
using System;

public class RewardSpawnManager: MonoBehaviour
{
    [SerializeField] private Transform leftSpawnLocation;
    [SerializeField] private Transform middleSpawnLocation;
    [SerializeField] private Transform rightSpawnLocation;
    [SerializeField]  private float spawnOffset = 0.5f;
    [SerializeField] private Chest chest;
    private Transform rightHandSlot => PlayerController.Instance.InventoryCmp.RightHandSlot;
    private List<Transform> spawnLocationList;

    private void Awake()
    {
        GlobalEventManager.OnGivingPlayerBranchTree += GlobalEventManager_OnGivingPlayerBranchTree1;
    }
    private void OnDisable()
    {
        GlobalEventManager.OnGivingPlayerBranchTree -= GlobalEventManager_OnGivingPlayerBranchTree1;
    }
    private void GlobalEventManager_OnGivingPlayerBranchTree1(object sender, GameObject branchTreePrefab)
    {
        GameObject newWeapon = Instantiate(branchTreePrefab, rightHandSlot);
        newWeapon.layer = LayerMask.NameToLayer(GameConstants.EquippedWeaponLayer);
        newWeapon.transform.localPosition = Vector3.zero;
        PlayerController.Instance.InventoryCmp.CurrentWeapon = newWeapon.GetComponent<Weapon>();
    }



    private void Start()
    {
        Initialize();
    }

    public void SpawnRewards(WeaponStatSO[] rewardArray)
    {
        //loop through spawnLocationList to instantiate rewards
        if (spawnLocationList.Count == 0) return;
        foreach (WeaponStatSO element in rewardArray)
        {
            var rewardPrefab = element.weaponPrefab; 
            var location = spawnLocationList[0];
            var rewardObj = Instantiate(rewardPrefab,location);
            var weaponCmp = rewardObj.GetComponent<Weapon>();
            if (weaponCmp != null)
            {
                weaponCmp.OnWeaponPickedUp += WeaponCmp_OnWeaponPickedUp;
            }
            rewardObj.transform.position += new Vector3(0f, spawnOffset, 0f);
            spawnLocationList.RemoveAt(0);
        }
    }
    private void Initialize()
    {
        //create spawnLocationList
        if (spawnLocationList == null)
        {
            spawnLocationList = new List<Transform>{ leftSpawnLocation, middleSpawnLocation, rightSpawnLocation };
        }
    }
    private void WeaponCmp_OnWeaponPickedUp(object sender, EventArgs args)
    {
        chest.GettingFirstWeaponQuest.isCompleted = true;
        Debug.Log("Quest Completed");
    }
}
