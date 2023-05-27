using RPG.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.VFX
{
    public class TakeDamageEffectPool : MonoBehaviour, IPool
    {
        //rewrite takedamageEffectPool using HealingEffectPool script
        [SerializeField] private int capacity = 3;
        [SerializeField] private int maxSize = 10;
        [SerializeField] private GameObject takeDmgEffectPrefab;
        private ObjectPool<GameObject> takeDmgEffectPool;
        public ObjectPool<GameObject> Pool => takeDmgEffectPool;
        private bool collectionChecks = true;

        private void Start()
        {
            takeDmgEffectPool = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity, maxSize);
        }

        public GameObject CreatePoolObject()
        {
            var takeDmgEffect = Instantiate(takeDmgEffectPrefab);
            takeDmgEffect.GetComponent<TakeDamageEffect>().Pool = takeDmgEffectPool;
            takeDmgEffect.SetActive(false);
            return takeDmgEffect;
        }

        public void OnDestroyPoolObject(GameObject gameObject)
        {
            Destroy(gameObject);

        }

        public void OnReturnedToPool(GameObject takeDmgEffect)
        {
            takeDmgEffect.SetActive(false);

        }

        public void OnTakeFromPool(GameObject takeDmgEffect)
        {
            takeDmgEffect.GetComponent<TakeDamageEffect>().Pool = takeDmgEffectPool;
            takeDmgEffect.SetActive(true);
        }
    }
}





















//public class TakeDamageEffectPool : MonoBehaviour
//{
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
