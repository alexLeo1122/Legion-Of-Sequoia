using UnityEngine;
using UnityEngine.InputSystem;
using RPG.Character;
using RPG.Ultilities;
using System.Collections;

namespace RPG.Quest
{
    public class NPCSlime : MonoBehaviour
    {
        [SerializeField] private NPCInteracableSign interacableSign;
        [SerializeField] private GameObject followSign;
        public bool IsPlayerInRange { get; set; }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(GameConstants.PlayerTag))
            {
                IsPlayerInRange = true;
                interacableSign.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(GameConstants.PlayerTag))
            {
                IsPlayerInRange = false;
                interacableSign.gameObject.SetActive(false);
            }
        }
        private void Start()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Interact.started += HandlePlayerInteract;
        }

        private void OnDisable()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Interact.started -= HandlePlayerInteract;
        }
        private void HandlePlayerInteract(InputAction.CallbackContext obj)
        {
            if (!IsPlayerInRange) return;
            StartCoroutine(NPCFollowPlayer());
        }
        private IEnumerator NPCFollowPlayer()
        {
            followSign.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            GlobalEventManager.OnSlimeFollowHeroRaised();
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }

    }
}

