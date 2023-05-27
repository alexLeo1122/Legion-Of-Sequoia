using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Character 
{
    public abstract class GameCharacterController : MonoBehaviour
    {
        //Test
        [SerializeField] private Transform effectPoint;
        public Transform EffectPoint => effectPoint;

        public virtual Vector3 DirectionVector { get; protected set; }
        public NavMeshAgent Agent { get; private set; }

        //Manage SpeedBlend Anim Params for Character
        protected struct MoveSpeedBlend
        {
            public float value;
            public float accelerate;
            public float decelerate;
            public MoveSpeedBlend(float value, float acclerate, float deccelerate)
            {
                this.value = value;
                accelerate = acclerate;
                decelerate = deccelerate;
            }

            public float GetDecelerateValue()
            {
                return decelerate * Time.deltaTime;
            }
            public float GetAccelerateValue()
            {
                return accelerate * Time.deltaTime;
            }
        }
        protected MoveSpeedBlend speedBlend = new MoveSpeedBlend(0f, 5f, 5f * 1.5f);
        [NonSerialized] public bool hasResetSpeedBlend = false;

        //---------------
        public void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
        }
        public virtual void Start()
        {
            InitializeStats();
        }
        public virtual void Update()
        {
            RotateAgent(DirectionVector);
            UpdateChildComponentParams();
        }

        //------------------------
        public abstract void InitializeStats();
        public virtual void RotateAgent(Vector3 directionVector)
        {
            if (directionVector == Vector3.zero)
            {
                return;
            }
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(directionVector);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, Agent.angularSpeed * Time.deltaTime);
        }
        public abstract void UpdateChildComponentParams();
        public virtual void SetDirectionVector(Vector3 newDirection)
        {
            DirectionVector = newDirection;
        }

        public virtual float DistanceToTarget(Vector3 target)
        {
            return Vector3.Distance(transform.position, target);
        }
        public virtual void HandleAfterDefeated (float sec = 1.8f)
        {
            StartCoroutine(HandleDefeatedAfterSec(sec));
        }

        //Coroutine
        public virtual IEnumerator HandleDefeatedAfterSec(float sec)
        {
            yield return new WaitForSeconds(sec);
            Destroy(gameObject);
        }
        public float GetSpeedBlend()
        {
            return speedBlend.value;
        }

    }
}



