using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections;
using RPG.Ultilities;
using System.Collections.Generic;

namespace RPG.Character
{
    public class ProjectilePool : MonoBehaviour
    //, IPool
    {

        #region Declarations
        [SerializeField]
        private int capacity = 10;
        [SerializeField]
        private int maxSize = 15;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private BossController bossController;
        private bool collectionChecks = true;
        //public ObjectPool<GameObject> projectilePool;
        //public ObjectPool<GameObject> Pool => projectilePool;
        public ObjectPool<GameObject> pool1;
        public ObjectPool<GameObject> pool2;
        public ObjectPool<GameObject> Pool1 => pool1;
        public ObjectPool<GameObject> Pool2 => pool2;
        public ObjectPool<GameObject> CurrentPool {get; set;}

        #endregion

        #region Unity Methods
        private void Awake()
        {
            BossEventManager.OnReturnProjectileToPool += BossEventManager_OnReturnProjectileToPool;
        }
        private void OnDisable()
        {
            BossEventManager.OnReturnProjectileToPool -= BossEventManager_OnReturnProjectileToPool;
        }
        private void BossEventManager_OnReturnProjectileToPool(object sender, BossEventManager.OnReturnProjectileToPoolArgs e)
        {
            e.fireball.transform.position = bossController.CombatCmp.FirePoint.position;
            GetCurrentPool().Release(e.fireball.gameObject);
        }

        private void Start()
        {
            pool1 = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity/2, maxSize/2);
            pool2 = new ObjectPool<GameObject>(CreatePoolObject, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, capacity / 2, maxSize/2);
            PolpulatePool();
            CurrentPool = pool1;
        }
        #endregion
        #region Custom Methods
        public void PolpulatePool()
        {

            for (int num = 0; num < capacity / 2; num++)
            {
                var poolObject = CreatePoolObject();
                //poolObject.SetActive(true);
                pool1.Release(poolObject);
            }
            for (int num = 0; num < capacity / 2; num++)
            {
                var poolObject = CreatePoolObject();
                //poolObject.SetActive(true);
                pool2.Release(poolObject);
            }
        }
        //
        public ObjectPool<GameObject> GetCurrentPool()
        {
            SwitchPool();
            return CurrentPool;
        }
        public void SwitchPool()
        {
            if (CurrentPool == pool1)
            {
                CurrentPool = pool2;
            }
            else
            {
                CurrentPool = pool1;
            }
        }
        public GameObject CreatePoolObject()
        {
                var projectile = Instantiate(projectilePrefab);
                //projectile.SetActive(false);
                return projectile;
        }

        public void OnDestroyPoolObject(GameObject projectile)
        {
            Destroy(projectile);
        }

        public void OnTakeFromPool(GameObject projectile)
        {
            BossEventManager.OnSettingProjectileDataRaised(projectile, new BossEventManager.OnSettingProjectileDataArgs() { fireball = projectile.GetComponent<Fireball>() }); ;
            projectile.SetActive(true);
        }

        public void OnReturnedToPool(GameObject projectile)
        {
            projectile.SetActive(false);
        }


        #endregion

        #region Event Handlers

        #endregion
    }

}

