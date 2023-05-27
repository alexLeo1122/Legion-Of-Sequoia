using RPG.Character;
using RPG.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxVisual : MonoBehaviour
{
    private Animator animatorCmp;
    private FoxController controller;
    private void Awake()
    {
        animatorCmp = GetComponent<Animator>();
        controller = GetComponentInParent<FoxController>();
        GlobalEventManager.OnFoxBeingDefeated += GlobalEventManager_OnFoxBeingDefeated;
    }
    private void OnDisable()
    {
        GlobalEventManager.OnFoxBeingDefeated -= GlobalEventManager_OnFoxBeingDefeated;
    }


    private void GlobalEventManager_OnFoxBeingDefeated(object sender, EventArgs e)
    {
        animatorCmp.SetTrigger("isDefeated");
    }

    public void HandleFoxRunAnim()
    {
        animatorCmp.SetBool("isRunning", true);
    }
    public void HandleFoxIdleAnim()
    {
        animatorCmp.SetBool("isRunning", false);
    }
    public void HandleFoxAttackAnim()
    {
        animatorCmp.SetBool("isRunning", false);
        animatorCmp.SetTrigger("attack");
    }

    #region Animation Event
    public void OnFoxAttackLand()
    {
        PlayerController.Instance.HealthCmp.TakeDamage(controller.Damage);
    }
    public void OnFoxAttackEnd()
    {
        controller.IsAttacking = false;
        controller.StateMachine.TransitionToState(RPG.Ultilities.FoxStateEnum.FoxIdleState);
    }


    private void OnFoxDefeatedAnimEnd()
    {
        controller.IsDefeatedAnimEnd = true;
    }
    #endregion
}
