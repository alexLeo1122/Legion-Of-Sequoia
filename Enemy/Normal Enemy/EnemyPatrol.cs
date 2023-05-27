using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using RPG.Ultilities;


namespace RPG.Character
{
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private SplineAnimate splineAnimateCmp;
        [SerializeField] private EnemyController enemyController;
        private float offsetTime = 0.5f;
        public bool IsAlreadyPatrolling { get; set; }
        public bool IsResetSplinePath {  get; set; }

        private void Awake()
        {
            splineAnimateCmp.MaxSpeed = enemyController.EnemyStatSO.patrolSpeed;
        }
        public  IEnumerator  WaitBeforStartPatrolling()
        {
            yield return new WaitForSeconds(offsetTime);
            enemyController.StateMachine.TransitionToState(EnemyStateEnum.EnemyPatrolState);
        }
        public void Patrol()
        {
            //only Patrol (Play SplineAnimate) if
            //if is not already Patrolling (SplineAnimate is not currently playing)
            // and spline path has been reset (enemy back to original position);
            if (IsAlreadyPatrolling) return;
            if (!IsResetSplinePath) { 
                 splineAnimateCmp.Restart(true);
                IsResetSplinePath = true;
            }
            splineAnimateCmp.Play();
            IsAlreadyPatrolling = true;
        }
        public void StopPatrolling()
        {
            splineAnimateCmp.Pause();
        }

        public void Initiallize()
        {
            StartCoroutine(WaitBeforStartPatrolling());
        }
    }

}

