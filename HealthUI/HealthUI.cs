using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
    public class HealthUI : MonoBehaviour
    {
        [ SerializeField] private float healthBarOffset = 0.85f;
        private CharacterHealth characterHealth;
        [SerializeField] private Image healthImage;
        [SerializeField] private Image backgroundImage;
        private Color healthImgOriginalColor;
        private Color healthImgSavedColor;
        private Color backgroundImgOriginalColor;
        public Gradient newGradient;
        public float decrementTime = 0.2f;
        public float HealthDecrementTime => decrementTime;
        private float currentHealthRatio = 1f;
        private float targetHealthRatio = 0f;

        public void HandleHealthRatioChanged(object sender, CharacterHealth.OnHealthRatioChangedArgs e)
        {
            StartCoroutine(HealthDecrementSmoothening(e.healthRatio));
        }

        public  IEnumerator  HealthDecrementSmoothening(float ratio)
        {
            targetHealthRatio = ratio;
            float elapstedTime = 0f;
            while (elapstedTime < decrementTime)
            {
                elapstedTime += Time.deltaTime;
                healthImage.fillAmount = Mathf.Lerp(currentHealthRatio, targetHealthRatio, elapstedTime/decrementTime);
                healthImage.color = newGradient.Evaluate(healthImage.fillAmount);
                yield return null;
            }
            healthImage.fillAmount = ratio;
            currentHealthRatio = ratio;
        }
        private void Start()
        {
            healthImgOriginalColor = healthImage.color;
            healthImgSavedColor = healthImage.color;
            backgroundImgOriginalColor = backgroundImage.color;
        }
        private void LateUpdate()
        {
            SetHealthUIPosition();
        }

        public void SetHealthCmp (CharacterHealth healthCmp)
        {
            this.characterHealth = healthCmp;
        }
        public void HideHealthUI()
        {
            healthImgOriginalColor = healthImage.color;
            healthImage.color = Color.clear;
            backgroundImage.color = Color.clear;
        }
        public void ShowHealthUI()
        {
            healthImage.color = healthImgOriginalColor;
            if (healthImage.color == Color.clear)
            {
                healthImage.color = healthImgSavedColor;
            }
            backgroundImage.color = backgroundImgOriginalColor;
        }

        public void DestroyHealthUI()
        {
            if (this == null) return;
            Destroy(gameObject);
        }

        public void SetHealthUIPosition ()
        {
            if (characterHealth == null) return;
            transform.position = Camera.main.WorldToScreenPoint(characterHealth.transform.position + Vector3.up * healthBarOffset);
        }
    }

}

