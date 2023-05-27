using RPG.Character;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.VFX
{
    public class HealingEffectPool : MonoBehaviour, IPool
    {
        [SerializeField] private int capacity = 3;
        [SerializeField] private int maxSize = 10;
        [SerializeField] private GameObject healingEffectPrefab;
        private ObjectPool<GameObject> healingEffectPool;
        public ObjectPool<GameObject> Pool => healingEffectPool;

        private bool collectionChecks = true;
        private void Start()
        {
            healingEffectPool = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity, maxSize);
        }
        public GameObject CreatePoolObject()
        {
            var healingEffect = Instantiate(healingEffectPrefab);
            healingEffect.GetComponent<HealingEffect>().Pool = healingEffectPool;
            healingEffect.SetActive(false);
            return healingEffect;
        }
        public void OnDestroyPoolObject(GameObject healingEffect)
        {
            Destroy(healingEffect);
        }
        public void OnTakeFromPool(GameObject healingEffect)
        {
            healingEffect.GetComponent<HealingEffect>().Pool = healingEffectPool;
            healingEffect.SetActive(true);
        }
        public void OnReturnedToPool(GameObject healingEffect)
        {
            healingEffect.SetActive(false);
        }
    }
}



