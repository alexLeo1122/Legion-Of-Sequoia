
using UnityEngine;
using RPG.Ultilities;
using System.Collections;

namespace RPG.Character
{
    public class Fireball : MonoBehaviour
    {

        [SerializeField] private float speed = 0.5f;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private float returnToPoolTime = 5f;
        private bool isHittingPlayer = false;
        private bool isReturningToPool = false;
        private float returnAfterExplosionTime = 1f;
       
        public Vector3 FireballDirection { get; set; }
        private void OnEnable()
        {
            Initialize();
        }
        private void OnDisable()
        {
            //StopAllCoroutines();
        }
        private void Update()
        {
            if (isHittingPlayer || isReturningToPool) return;
            HandleFireballMove();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GameConstants.PlayerHitColliderLayer))
            {
                trailPrefab.SetActive(false);
                fireballPrefab.SetActive(false);
                Vector3 explosionPosition = GetExplosionPosition(other);
                explosionPrefab.transform.position = explosionPosition;
                explosionPrefab.SetActive(true);
                isHittingPlayer = true;
                StopCoroutine(HandleReturnToPoolAfterTime());
                StartCoroutine(HandleFireballHitTarget(other.gameObject));
            }
        }
        private void HandleFireballMove()
        {
            transform.Translate(FireballDirection * speed * Time.deltaTime,Space.World);
            //transform.rotation = Quaternion.LookRotation(FireballDirection);
            //fireBall.transform.forward = fireBall.FireballDirection;
        }
        private IEnumerator HandleFireballHitTarget(GameObject target)
        {
            BossEventManager.OnProjectileHitTargetRaised(target);
            yield return new WaitForSeconds(returnAfterExplosionTime);
            ReturnToPool();
        }
        private IEnumerator HandleReturnToPoolAfterTime()
        {
            yield return new WaitForSeconds(returnToPoolTime);
            ReturnToPool();
        }
        public void Initialize()
        {
            StartCoroutine(HandleReturnToPoolAfterTime());
            isReturningToPool = false;
            isHittingPlayer = false;
            explosionPrefab.SetActive(false);
            fireballPrefab.SetActive(true);
            StartCoroutine(ResetTrail());
        }

        public IEnumerator ResetTrail()
        {
            trailPrefab.SetActive(false);
            yield return new WaitForSeconds(0.15f);
            trailPrefab.SetActive(true);
        }

        private void ReturnToPool()
        {
            trailPrefab.SetActive(false);
            fireballPrefab.SetActive(false);
            isReturningToPool = true;
            BossEventManager.OnReturnProjectileToPoolRaised(this.gameObject, new BossEventManager.OnReturnProjectileToPoolArgs() {fireball = this});
        }
        private Vector3 GetExplosionPosition(Collider target)
        {
            Vector3 explosionPos = Vector3.zero;
            var distanceVector = (target.gameObject.transform.position - explosionPrefab.transform.position) * 1 / 3;
            explosionPos = new Vector3 
            (
                explosionPrefab.transform.position.x + distanceVector.x,
                explosionPrefab.transform.position.y,
                explosionPrefab.transform.position.z + distanceVector.z
            );
            return explosionPos;
        }
    }

}
