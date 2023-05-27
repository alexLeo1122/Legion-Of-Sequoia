using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace RPG.Character
{
    public class LightManager : MonoBehaviour
    {
        [SerializeField] private Light dLightMorning;
        [SerializeField] private Light dLightEvening;
        private float lightChangingDuration = 1.75f;
        private void Awake()
        {
            GlobalEventManager.OnSwitchingBattlefieldLight += Switch;
        }
        public void Switch(object sender, EventArgs args)
        {
            StartCoroutine(SwitchingLightSmoothly(dLightMorning, dLightEvening));
        }
        private void OnDisable()
        {
            GlobalEventManager.OnSwitchingBattlefieldLight -= Switch;
        }
        IEnumerator SwitchingLightSmoothly(Light currentlight, Light targetLight)
        {
            float startingTemperature = currentlight.colorTemperature;
            Color startingFilter = currentlight.color;
            float targetTemperature = targetLight.colorTemperature;
            Color targetFilter = targetLight.color;
            float elapsedTime = 0f;
            while (elapsedTime < lightChangingDuration)
            {
                elapsedTime += Time.deltaTime;
                float timeRatio = elapsedTime / lightChangingDuration;
                currentlight.colorTemperature = Mathf.Lerp(startingTemperature, targetTemperature, timeRatio);
                currentlight.color = Color.Lerp(startingFilter, targetFilter, timeRatio);
                yield return null;
            }
        }


    }

}



