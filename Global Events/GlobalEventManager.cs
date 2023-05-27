using System;
using UnityEngine;
using RPG.Terrain;

namespace RPG.Character
{
    public class GlobalEventManager
    {
        #region Events
        public static event EventHandler OnSwitchingBattlefieldLight;
        public static event EventHandler<TextAsset> OnInitiateDialogue;
        public static event EventHandler<bool> OnCurrentQuestDialogueEnd;
        public static event EventHandler<OnResetStoryArgs> OnResetStory;
        public static event EventHandler OnPlayerAchievedQuestMission;
        public static event EventHandler<OnFollowPlayerMoveArgs> OnAllyFollowPlayerMove;
        public static event EventHandler<OnSlimePerformAblilityArgs> OnSlimePerformAbility;
        public static event EventHandler<GameObject> OnGivingPlayerBranchTree;
        public static event EventHandler OnWeaponChestOpen;
        public static event EventHandler<CharacterHealth> OnHidingHealthBarUI;
        public static event EventHandler<CharacterHealth> OnShowingHealthBarUI; 
        public static event EventHandler<OnPlayerEnterAttackRangeArgs> OnPlayerEnterAttackRange;
        public static event EventHandler<OnPlayerExitAttackRangeArgs> OnPlayerExitAttackRange;
        public static event EventHandler OnFoxBeingDefeated;
        public static event EventHandler OnSlimeFollowHero;
        public static event EventHandler OnReceivingSlimeBackCompleted;
        public static event EventHandler OnPlayerFindingTreasureChest;
        public static event EventHandler OnPickingUpWeapon;
        public static event EventHandler OnSkelMageBeingDefeated;
        public static event EventHandler<bool> OnGameEnd;
        public static event EventHandler OnHealingTakeEffect;
        public static event EventHandler<GameCharacterController> OnPlayTakeDamageEffect;
        public static event EventHandler<Transform> OnGameObjectDisappeared;
        public static event EventHandler<Transform> OnBossSummonedEnemies;
        public static event EventHandler<TreeVisual> OnTreeAppeared;
        public static bool HasGameRestarted { get; set; } = false;
        #endregion

        public class OnResetStoryArgs : EventArgs
        {
            public TextAsset dialogueFile;
            public bool hasMissionAchieved;
        }
        public class OnFollowPlayerMoveArgs : EventArgs 
        {
            public Vector3 movementVector;
            public float playerSpeed;
        }
        public class OnSlimePerformAblilityArgs : EventArgs
        {
            public float damage;
            public CharacterHealth target;
        }

        public class OnPlayerEnterAttackRangeArgs : EventArgs
        {
            public GameObject enemy;
        }
        public class OnPlayerExitAttackRangeArgs : EventArgs
        {
            public GameObject enemy;
        }

        #region Raise Events Methods    
        public static void OnSwitchBattlefieldLightRaised()
        {
            OnSwitchingBattlefieldLight?.Invoke(null, EventArgs.Empty);
        }

        public static void OnInitiateDialogueRaised(TextAsset dialogueFile)
        {
            OnInitiateDialogue?.Invoke(null, dialogueFile);
        }
        public static void OnCurrentDialogueEndRaised(bool hasMissionAchieved)
        {
            OnCurrentQuestDialogueEnd?.Invoke(null, hasMissionAchieved);
        }
        public static void OnResetStoryRaised(OnResetStoryArgs args)
        {
            OnResetStory?.Invoke(null, args);
        }
        public static void OnPlayerAchievedQuestMissionRaised()
        {
            OnPlayerAchievedQuestMission?.Invoke(null, EventArgs.Empty);
        }

        public static void OnFollowPlayerMovedRaised(object sender, OnFollowPlayerMoveArgs args)
        {
            OnAllyFollowPlayerMove?.Invoke(sender, args);
        }

        public static void OnSlimePerformAbilityRaised(object sender, OnSlimePerformAblilityArgs args)
        {
            OnSlimePerformAbility?.Invoke(sender, args);
        }

        public static void OnGivingPlayerBranchTreeRaised(GameObject branchTreePrefab)
        {
            OnGivingPlayerBranchTree?.Invoke(null, branchTreePrefab);
        }
        public static void OnWeaponChestOpenRaised()
        {
            OnWeaponChestOpen?.Invoke(null, EventArgs.Empty);
        }
        public static void OnHidingHealthBarUIRaised(CharacterHealth characterHealth)
        {
            OnHidingHealthBarUI?.Invoke(null, characterHealth);
        }
        public static void OnShowingHealthBarUIRaised(CharacterHealth characterHealth)
        {
            OnShowingHealthBarUI?.Invoke(null, characterHealth);
        }
        public static void OnPlayerEnterAttackRangeRaised(GameObject enemy)
        {
            OnPlayerEnterAttackRange?.Invoke(null, new OnPlayerEnterAttackRangeArgs() { enemy = enemy});
        }

        public static void OnPlayerExitAttackRangeRaised(GameObject gameObject)
        {
            OnPlayerExitAttackRange?.Invoke(null, new OnPlayerExitAttackRangeArgs() { enemy = gameObject });
        }
        public static void OnFoxBeingDefeatedRaised()
        {
            OnFoxBeingDefeated?.Invoke(null, EventArgs.Empty);
        }
        public static void OnSlimeFollowHeroRaised()
        {
            OnSlimeFollowHero?.Invoke(null, EventArgs.Empty);
        }   
        public static void OnReceivingSlimeBackCompleteRaised()
        {
            OnReceivingSlimeBackCompleted?.Invoke(null, EventArgs.Empty);
        }
        public static void OnPlayerFindingTreasureChestRaised()
        {
            OnPlayerFindingTreasureChest?.Invoke(null, EventArgs.Empty);
        }
        public static void OnPickingUpWeaponRaised()
        {
            OnPickingUpWeapon?.Invoke(null, EventArgs.Empty);
        }
        public static void OnSkelMageBeingDefeatedRaised()
        {
            OnSkelMageBeingDefeated?.Invoke(null, EventArgs.Empty);
        }
        public static void OnGameEndRaised(bool result)
        {
            OnGameEnd?.Invoke(null, result);
        }
        public static void OnHealingTakeEffectRaised()
        {
            OnHealingTakeEffect?.Invoke(null, EventArgs.Empty);
        }
        public static void OnPlayTakeDamageEffectRaised(GameCharacterController controller)
        {
            OnPlayTakeDamageEffect?.Invoke(null, controller);
        }
        public static void OnGameObjectDisappearedRaised(Transform transform)
        {
            OnGameObjectDisappeared?.Invoke(null, transform);
        }
        public static void OnBossSummonedEnemiesRaised(Transform transform)
        {
            OnBossSummonedEnemies?.Invoke(null, transform);
        }
        public static void OnTreeAppearedRaised(TreeVisual treeVisual)
        {
            OnTreeAppeared?.Invoke(null, treeVisual);
        }
        #endregion
    }
}

