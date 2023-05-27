using RPG.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BossTeleport : MonoBehaviour
{

    public void RemoveAgent()
    {
        var agent = GetComponent<NavMeshAgent>();
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
            transform.position = GetComponent<BossMovement>().TeleportPos.transform.position;
            // navMeshAgent.enabled = true;
        }
    }
}
