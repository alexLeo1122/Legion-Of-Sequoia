using RPG.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quest
{
    public class EnemyAttackRange : MonoBehaviour
    {
        private FoxController controller;
        private void Awake()
        {
            controller = GetComponentInParent<FoxController>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHitCollider"))
            {
                GlobalEventManager.OnPlayerEnterAttackRangeRaised(this.gameObject);
                controller.IsPlayerInAttackRange = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerHitCollider"))
            {
                GlobalEventManager.OnPlayerExitAttackRangeRaised(this.gameObject);
                controller.IsPlayerInAttackRange = false;   
            }
        }
    }
}


