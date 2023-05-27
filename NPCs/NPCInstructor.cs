using UnityEngine;
using RPG.UI;
using RPG.Quest;
using System;
using System.Collections;
using RPG.Ultilities;

namespace RPG.Character
{
    public class NPCInstructor : MonoBehaviour
    {

        #region Declarations
        [SerializeField] QuestManager questManager;
        [SerializeField] NPCInteracableSign interacableSign;
        [SerializeField] GameObject instructorVisual;
        [SerializeField] GameObject branchTreePrefab;
        [SerializeField] Transform teleportPos;
        private QuestSO currentQuest => questManager.CurrentQuest;
        private TextAsset currentStory => currentQuest.questStory;
        private float timeBeforeWelcome = 0.5f;
        public bool IsPlayerInRange { get; set; } = false;
        public bool HasMissionAchived { get; set; }
        private PlayerController playerController => PlayerController.Instance;
        private float elapsedTime = 0f;
        private float rotateTime = 0.2f;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            GlobalEventManager.OnCurrentQuestDialogueEnd += HandleQuestDialogueEnd;
            GlobalEventManager.OnPlayerAchievedQuestMission += HandlePlayerAchievedQuestMission;
            GlobalEventManager.OnSlimeFollowHero += HandlePlayerGuidingSlimeBack;
            GlobalEventManager.OnPickingUpWeapon += GlobalEventManager_OnPickingUpWeapon;
            GlobalEventManager.OnSkelMageBeingDefeated += GlobalEventManager_OnSkelMageBeingDefeated;
        }
        private void Start()
        {
            PlayerController.Instance.GameInputSO.GameInput.Player.Interact.started += HandlePlayerInteract;
            Initialize();
            StartCoroutine(WelcomePlayer());
        }

        private void GlobalEventManager_OnSkelMageBeingDefeated(object sender, EventArgs e)
        {
            HasMissionAchived = true;
        }

        private void GlobalEventManager_OnPickingUpWeapon(object sender, EventArgs e)
        {
            HasMissionAchived = true;
        }

        private void HandlePlayerGuidingSlimeBack(object sender, EventArgs e)
        {
            HasMissionAchived = true;
        }



        //private void Update()
        //{
        //    Debug.Log($"Active Quest: {currentQuest.questName}, isComplete: {currentQuest.isComplete}");
        //}
        private void OnDisable()
        {
            GlobalEventManager.OnCurrentQuestDialogueEnd -= HandleQuestDialogueEnd;
            GlobalEventManager.OnPlayerAchievedQuestMission -= HandlePlayerAchievedQuestMission;
            GlobalEventManager.OnSlimeFollowHero -= HandlePlayerGuidingSlimeBack;
            GlobalEventManager.OnPickingUpWeapon -= GlobalEventManager_OnPickingUpWeapon;
            PlayerController.Instance.GameInputSO.GameInput.Player.Interact.started -= HandlePlayerInteract;
        }

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

        #endregion

        #region Event Handlers

        private void HandlePlayerInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!IsPlayerInRange) return;
            if (currentQuest.isComplete) return;
            StartCoroutine(RotateToWardsPlayer());
            GlobalEventManager.OnResetStoryRaised(
                new GlobalEventManager.OnResetStoryArgs() { dialogueFile = currentStory, hasMissionAchieved = HasMissionAchived });
        }
        private void HandlePlayerAchievedQuestMission(object sender, EventArgs e)
        {
            HasMissionAchived = true;
        }

        private void HandleQuestDialogueEnd(object sender, bool hasMissionAchieved)
        {
            currentQuest.isComplete = hasMissionAchieved;
            if (!currentQuest.isComplete) return;
            currentQuest.isActive = false;
            HandlingQuestComplete();
            questManager.TransitionToNextQuest();
            Initialize();
        }

        #endregion

        #region Custom Methods
        private IEnumerator WelcomePlayer()
        {
            yield return new WaitForSeconds(timeBeforeWelcome);
            GlobalEventManager.OnInitiateDialogueRaised(currentStory);
        }
        private void Initialize()
        {
            HasMissionAchived = false;
        }
        private IEnumerator RotateToWardsPlayer()
        {
            elapsedTime = 0f;
            Vector3 directionVector = playerController.transform.position - instructorVisual.transform.position;
            directionVector.y = 0;
            Quaternion startRotation = instructorVisual.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(directionVector);
            while (elapsedTime <= rotateTime)
            {
                instructorVisual.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotateTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        private void  HandlingQuestComplete()
        {
            if (currentQuest.questName == GameConstants.NPCWelcomeQuest)
            {
                GlobalEventManager.OnGivingPlayerBranchTreeRaised(branchTreePrefab);
            }
            else if (currentQuest.questName == GameConstants.NPCSlimeQuest)
            {
                PlayerController.Instance.PlayerAbilityManager.UnlockAbility(PlayerAbilityEnum.HealingSpell);
                GlobalEventManager.OnReceivingSlimeBackCompleteRaised();

            }
            else if (currentQuest.questName == GameConstants.NPCIntroduceGettingWeapon)
            {
                GlobalEventManager.OnPlayerFindingTreasureChestRaised();
                PlayerController.Instance.PlayerAbilityManager.UnlockAbility(PlayerAbilityEnum.PickingUp);
            }
            else if (currentQuest.questName == GameConstants.NPCGettingFirstWeaponQuest)
            {
                PlayerController.Instance.PlayerAbilityManager.UnlockAbility(PlayerAbilityEnum.DashFrontAttack);
                PlayerController.Instance.PlayerAbilityManager.UnlockAbility(PlayerAbilityEnum.HeavyAttack);
            }
            else if (currentQuest.questName == GameConstants.NPCIntroduceDefeatingSkelMageQuest)
            {
                GlobalEventManager.OnSlimeFollowHeroRaised();
                TeleportToNewPos();
            }
            else if (currentQuest.questName == GameConstants.NPCDefeatingSkelMageQuest)
            {
                Debug.Log("Game End");
                StartCoroutine(WaitAndEndGame());
            }
        }

        private void TeleportToNewPos()
        {
            GlobalEventManager.OnGameObjectDisappearedRaised(this.transform);
            transform.position = teleportPos.transform.position;
        }
        private IEnumerator WaitAndEndGame()
        {
            yield return new WaitForSeconds(0.5f);
            GlobalEventManager.OnGameEndRaised(true);
        }
        #endregion


    }

}

