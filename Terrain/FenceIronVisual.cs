using System;
using UnityEngine;
using RPG.Ultilities;
using RPG.Character;

public class FenceIronVisual : MonoBehaviour
{
    private Animator animatorCmp;

    public event EventHandler OnPlayerPassGatePosition;
    private void Start()
    {
        animatorCmp = GetComponent<Animator>();
    }

    public void CloseBattleFieldGate()
    {
        animatorCmp.SetTrigger("gateClosed");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(GameConstants.PlayerTag))
        {
            OnPlayerPassGatePosition?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnBattlefieldGateClosed()
    {
        GlobalEventManager.OnSwitchBattlefieldLightRaised();
    }

}
