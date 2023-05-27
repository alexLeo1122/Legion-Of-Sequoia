using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using RPG.Ultilities;

namespace RPG.Character
{
    public class SummonEnemyPool : MonoBehaviour, IPool
    {
        [SerializeField] private int capacity = 10;
        [SerializeField] private int maxSize = 15;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform leftSummonLocation;
        [SerializeField] private Transform rightSummonLocation;
        [SerializeField] private Transform middleLeftSummonLocation;
        [SerializeField] private Transform middleRightSummonLocation;
        private List<Transform> spawnLocationList;
        private ObjectPool<GameObject> summonEnemyPool;
        public ObjectPool<GameObject> Pool => summonEnemyPool;
        private bool collectionChecks = true;
        public List<GameObject> ActiveSummonEnemiesList { get; set; } = new List<GameObject>();
        private void OnEnable()
        {
            BossEventManager.OnReturnSummonedEnemyToPool += BossEventManager_OnReturnSummonedEnemyToPool;
        }
        private void OnDisable()
        {
            BossEventManager.OnReturnSummonedEnemyToPool -= BossEventManager_OnReturnSummonedEnemyToPool;
        }


        private void Start()
        {
            summonEnemyPool = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity, maxSize);
            spawnLocationList = new List<Transform>() { leftSummonLocation, rightSummonLocation, middleLeftSummonLocation, middleRightSummonLocation };
        }

        private void PolpulatePool() 
        { 
            for (int num = 0; num < capacity; num++)
            {
                var poolObject = CreatePoolObject();
                poolObject.SetActive(true);
                Pool.Release(poolObject);
            }
        }
        public GameObject CreatePoolObject()
        {
            var enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            return enemy;
        }
        public void OnDestroyPoolObject(GameObject enemy)
        {
            Destroy(enemy);
        }
        public void OnTakeFromPool(GameObject enemy)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyController>().IsSummonedEnemy = true;

        }
        public void OnReturnedToPool(GameObject enemy)
        {
            enemy.SetActive(false);
        }

        public void SummonEnemyInSpawnLocations()
        {
            var enemy = Pool.Get();
            Transform summonLocation = spawnLocationList.Shuffle()[0];
            enemy.transform.position = summonLocation.position;
            enemy.transform.rotation = Quaternion.identity;
            ActiveSummonEnemiesList.Add(enemy);
        }
        public void ReturnEnemyToPool(GameObject enemy)
        {
            Pool.Release(enemy);
            ActiveSummonEnemiesList.Remove(enemy);  
        }

        private void BossEventManager_OnReturnSummonedEnemyToPool(object sender, BossEventManager.OnReturnSummonedEnemyToPoolArgs e)
        {
            ReturnEnemyToPool(e.summonedEnemy);
        }
    }
}

