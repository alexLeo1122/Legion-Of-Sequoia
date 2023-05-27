using RPG.Character;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Ultilities
{
    public static class GameConstants
    {
        //QuestName
        public static string NPCWelcomeQuest = "NPCWelcomeQuest";
        public static string NPCSlimeQuest = "NPCSlimeQuest";
        public static string NPCIntroduceGettingWeapon = "NPCIntroduceGettingFirstWeaponQuest";
        public static string NPCGettingFirstWeaponQuest = "NPCGettingFirstWeaponQuest";
        public static string NPCIntroduceDefeatingSkelMageQuest = "NPCIntroduceDefeatingSkelMageQuest";
        public static string NPCDefeatingSkelMageQuest = "NPCDefeatingSkelMageQuest";
        public static string HasMissionAchievedInkVar = "hasMissionAchieved";


        //Animation constants for Characters
        public static string MoveSpeedBlend = "moveSpeedBlend";
        public static string DefaultAttackTriggerAnim = "attack";
        public static string HeavyAttackTriggerAnim = "heavyAttack";
        public static string HealingSpellTriggerAnim = "healingSpell";
        public static string PickingUpTriggerAnim = "pickingUp";
        public static string IsDefeatedBoolAnim = "isDefeated";
        public static string DashFrontAttackTriggerAnim = "dashFrontAttack";

        //Animation constants for Bosses 
        public static string FireBallSpellTriggerAnim = "fireballSpell";
        public static string SummoningSpellTriggerAnim = "summoningSpell";



        //Animation constants for others objects (chests)
        public static string IsOpenBoolAnim = "isOpen";

        //Layer
        public static string WeaponLayer = "Weapon";
        public static string EquippedWeaponLayer = "EquippedWeapon";
        public static string PlayerLayer = "Player";
        public static string EnemyLayer = "Enemy";
        public static string ChestLayer = "Chest";
        public static string EnemyDefeatedLayer = "EnemyDefeated";
        public static string CharacterDefeatedLayer = "Defeated";
        public static string PlayerHitColliderLayer = "PlayerHitCollider";

        //Label 
        public static string WeaponAssetsSOLabel = "WeaponAssetsSO";


        //Tags
        public static string ChestTag = "Chest";
        public static string PlayerTag = "Player";
        public static string EnemyTag = "Enemy";

    }
    public static class Helpers
    {
        public static int StringToHash(string text)
        {
            return Animator.StringToHash(text);
        }
        public static void PrintObjectName(object obj)
        {
            Debug.Log("This is " + obj.GetType().Name);
        }

        //write a function to print out all the key value pair in a dictionary

        public static void PrintDictionary<T, U>(Dictionary<T, U> dict)
        {
            foreach (KeyValuePair<T, U> kvp in dict)
            {
                Debug.Log("Key: " + kvp.Key.ToString() + ", Value: " + kvp.Value.ToString());
            }
        }

        // write a function to print out all member of array
        public static void PrintEnumerable<T>(IEnumerable<T> array)
        {
            foreach (var item in array)
            {
                Debug.Log(item + " ->");
            }
        }


    }

    public enum EnemyStateEnum
    {
        EnemyIdleState,
        EnemyPatrolState,
        EnemyChaseState,
        EnemyReturningState,
        EnemyAttackState,
        EnemyDefeatedState
    }

    public enum PlayerStateEnum
    {
        PlayerIdleState,
        PlayerMoveState,
        PlayerPerformAbilityState,
        PlayerDefeatedState
    }

    public enum PlayerAbilityEnum
    {
        [TypeAbility(PlayerTypeAbilityEnum.Attack)]
        DefaultAttack,
        [TypeAbility(PlayerTypeAbilityEnum.Attack)]
        HeavyAttack,
        [TypeAbility(PlayerTypeAbilityEnum.Attack)]
        DashFrontAttack,
        [TypeAbility(PlayerTypeAbilityEnum.Spell)]
        HealingSpell,
        [TypeAbility(PlayerTypeAbilityEnum.PickingUp)]
        PickingUp
    }

    public enum PlayerTypeAbilityEnum
    {
        Attack,
        Spell,
        PickingUp
    }


    //Boss State Enum
    public enum BossStateEnum
    {
        BossIdleState,
        BossChaseState,
        BossReturningState,
        BossPerformAbilityState,
        BossDefeatedState
    }

    public enum BossAbilityEnum
    {
        DefaultAttack,
        FireBallSpell,
        SummoningSpell,
        ShinraTenseiSpell,
        HealingSpell,
    }
    public enum BossTypeAbilityEnum
    {
        Attack,
        Spell,
        PickingUp
    }

    public enum SlimeStateEnum
    {
        SlimeFollowState,
        SlimePerformAbilityState
    }
    public enum FoxStateEnum
    {
        FoxIdleState,
        FoxChasingState,
        FoxAttackState,
        FoxDefeatedState
    }






    public struct QuestTest
    {
        public int id;
        public string name;
        public string description;
        public bool isCompleted;
    }


}

