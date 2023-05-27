
using UnityEngine;
using RPG.Ultilities;

namespace RPG.Character
{
    public class EnemyHealth : CharacterHealth
    {
        private EnemyController enemyController;
        private  void Awake()
        {
            enemyController = GetComponent<EnemyController>();
        }
        public override void OnHealthDepleted()
        {
            ///<summary>
            ///Remove Enemy from EnemyMonitor list, so player cant attack this enemy anymore
            ///transition to EnemyDefeatedState to disable enemy action,
            ///and destroy/Deactive this gameObject after sec;
            ///</summary>
            ///
            PlayerController.Instance.CombatCmp.enemyMonitor.healthList.Remove(this);
            enemyController.gameObject.layer = LayerMask.NameToLayer(GameConstants.EnemyDefeatedLayer);
            enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyDefeatedState);
            enemyController.EnemyActionsSO.OnEnemyDefeatedRaised();
        }

    }

}

