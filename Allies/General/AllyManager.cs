using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.Character
{
    public class AllyManager : MonoBehaviour
    {
        [SerializeField] private GameObject slimeLeaf;
        public GameObject CurrentPet => slimeLeaf;

        private void Awake()
        {
            GlobalEventManager.OnSlimeFollowHero += GlobalEventManager_OnSpawnSlimeFollowPlayer;
            GlobalEventManager.OnReceivingSlimeBackCompleted += GlobalEventManager_OnReceivingSlimeBack;
        }
        private void OnDisable()
        {
            GlobalEventManager.OnSlimeFollowHero -= GlobalEventManager_OnSpawnSlimeFollowPlayer;
            GlobalEventManager.OnReceivingSlimeBackCompleted -= GlobalEventManager_OnReceivingSlimeBack;
        }

        private void GlobalEventManager_OnReceivingSlimeBack(object sender, System.EventArgs e)
        {
            slimeLeaf.SetActive(false);
        }

        private void GlobalEventManager_OnSpawnSlimeFollowPlayer(object sender, System.EventArgs e)
        {
            slimeLeaf.SetActive(true);
            slimeLeaf.transform.position = PlayerController.Instance.FollowPoint.position;
        }
    }

}


