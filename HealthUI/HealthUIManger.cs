
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Ultilities;
using System;

namespace RPG.Character
{
    public class HealthUIManger : MonoBehaviour
    {
        [SerializeField] private GameObject healthUIPrefab;
        [SerializeField] private Canvas globalCanvas;
        private Dictionary<CharacterHealth, HealthUI> healthUIdict;
        private void Awake()
        {
            healthUIdict = new Dictionary<CharacterHealth, HealthUI>();
        }
        private void OnEnable()
        {
            CharacterHealth.OnHealthComponentAppeared += CharacterHealth_OnHealthComponentAppeared;
            CharacterHealth.OnHealthRatioChanged += CharacterHealth_OnHealthRatioChanged;
            CharacterHealth.OnRemoveHealthUI += CharacterHealth_OnRemoveHealthUI;
            GlobalEventManager.OnHidingHealthBarUI += GlobalEventManager_OnHidingHealthBarUI;
            GlobalEventManager.OnShowingHealthBarUI += GlobalEventManager_OnShowingHealthBarUI;
        }
        private void OnDisable()
        {
            CharacterHealth.OnHealthComponentAppeared -= CharacterHealth_OnHealthComponentAppeared;
            CharacterHealth.OnHealthRatioChanged -= CharacterHealth_OnHealthRatioChanged;
            CharacterHealth.OnRemoveHealthUI -= CharacterHealth_OnRemoveHealthUI;
            GlobalEventManager.OnHidingHealthBarUI -= GlobalEventManager_OnHidingHealthBarUI;
            GlobalEventManager.OnShowingHealthBarUI -= GlobalEventManager_OnShowingHealthBarUI;
        }

        private void GlobalEventManager_OnShowingHealthBarUI(object sender, CharacterHealth healthCmp)
        {
            var healthUI = healthUIdict[healthCmp];
            healthUI.ShowHealthUI();
        }

        private void GlobalEventManager_OnHidingHealthBarUI(object sender, CharacterHealth healthCmp)
        {
            var healthUI = healthUIdict[healthCmp];
            healthUI.HideHealthUI();
        }

        private void CharacterHealth_OnRemoveHealthUI(object sender, CharacterHealth.OnRemoveHealthUIArgs e)
        {
            if (healthUIdict.ContainsKey(e.characterHealth))
            {
                var healthUI = healthUIdict[e.characterHealth];
                healthUI.DestroyHealthUI();
                healthUIdict.Remove(e.characterHealth);
            }
        }

        private void CharacterHealth_OnHealthRatioChanged(object sender, CharacterHealth.OnHealthRatioChangedArgs e)
        {
            if (healthUIdict.ContainsKey(e.characterHealth))
            {
                var healthUI = healthUIdict[e.characterHealth];
                healthUI.HandleHealthRatioChanged(sender, e);
            }
        }

        private void CharacterHealth_OnHealthComponentAppeared(object sender, CharacterHealth.OnHealthComponentAppearedArgs e)
        {
                if (healthUIdict.ContainsKey(e.healthCmp)) return;
            var healthUIGameObject = Instantiate(healthUIPrefab, globalCanvas.transform);
            if (healthUIGameObject.TryGetComponent(out HealthUI healthUICmp))
            {
                healthUICmp.SetHealthCmp(e.healthCmp);
                healthUIdict.Add(e.healthCmp, healthUICmp);
                e.healthCmp.HealthDecrementTime = healthUICmp.HealthDecrementTime;
            }

        }
    }

}

