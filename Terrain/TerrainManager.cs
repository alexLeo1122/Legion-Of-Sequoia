using System;
using UnityEngine;


namespace RPG.Character
{
    public class TerrainManager : MonoBehaviour
    {
        [SerializeField] private GameObject battleField;
        [SerializeField] private FenceIronVisual fenceIronGate;
        [SerializeField] private GameObject wallToTreasureChest;
        [SerializeField] private GameObject caveBlockingFence;
        private Collider battleFieldCollider;
        private Transform playerTransform;
        private bool isGateClosed = false;
        private void Start()
        {
            playerTransform = PlayerController.Instance.transform;
            battleFieldCollider = battleField.GetComponent<BoxCollider>();
            fenceIronGate.OnPlayerPassGatePosition += FenceIronGate_OnPlayerPassGatePosition;
            GlobalEventManager.OnPlayerFindingTreasureChest += GlobalEventManager_OnPlayerFindingTreasureChest;
            GlobalEventManager.OnSkelMageBeingDefeated += GlobalEventManager_OnSkelMageBeingDefeated;
        }

        private void GlobalEventManager_OnSkelMageBeingDefeated(object sender, EventArgs e)
        {
            //remove blocking gate to the next quest
            caveBlockingFence.SetActive(false);
        }

        private void GlobalEventManager_OnPlayerFindingTreasureChest(object sender, EventArgs e)
        {
            Destroy(wallToTreasureChest);

        }
        private void OnDisable()
        {
            fenceIronGate.OnPlayerPassGatePosition -= FenceIronGate_OnPlayerPassGatePosition;
            GlobalEventManager.OnPlayerFindingTreasureChest -= GlobalEventManager_OnPlayerFindingTreasureChest;
            GlobalEventManager.OnSkelMageBeingDefeated -= GlobalEventManager_OnSkelMageBeingDefeated;

        }
        private void Update()
        {
            if (isGateClosed) return;
            if (IsPlayerInsideTerrain())
            {
                fenceIronGate.CloseBattleFieldGate();
                isGateClosed = true;
            }
        }
        public bool IsPlayerInsideTerrain()
        {
            return battleFieldCollider.bounds.Contains(playerTransform.position);
         
        }
        private void FenceIronGate_OnPlayerPassGatePosition(object sender, EventArgs e)
        {
            if(IsPlayerInsideTerrain())
            {
                fenceIronGate.CloseBattleFieldGate();
            }
        }
        //Animation Event

    }

}




