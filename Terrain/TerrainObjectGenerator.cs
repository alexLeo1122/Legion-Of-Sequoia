using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Terrain
{
    public class TerrainObjectGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject navMesh;
        [SerializeField] private int numberMin = 15;
        [SerializeField] private int numberMax = 30;
        [SerializeField] private string folderName = "TreePrefab";
        private int numOfObjects;
        [SerializeField] private float sizeMin = 0.6f;
        [SerializeField] private float sizeMax = 1.7f;
        [SerializeField] private float spawnOffset = 1f;
        private float maxDistance = 3f;
        private Bounds navMeshBounds;
        private GameObject[] prefabArr;
        //TreePrefab    

        private void Start()
        {
            SetNavMeshBounds();
            SetNumOfObjects();
            SetPrefabArray();
            GenerateObjectOnNavMesh();
        }

        private void SetNumOfObjects()
        {
            numOfObjects = Random.Range(numberMin, numberMax);
        }
        public void SetNavMeshBounds()
        {
            var navMeshCollider = navMesh.GetComponent<Collider>();
            navMeshBounds = navMeshCollider.bounds;
        }
        public void GenerateObjectOnNavMesh()
        {
            //run for loop
            for (int num = 0; num < numOfObjects; num++)
            {
                Vector3 randomPosition = GetRandomPositionOnNavMesh();
                if (randomPosition == Vector3.zero) continue;
                //Generate Random prefab
                var objPrefab = GetRandomPrefab();
                GameObject terrainObj = Instantiate(objPrefab, randomPosition, Quaternion.identity);
                var visualGameObj = terrainObj.transform.GetChild(0);
                visualGameObj.localScale = new Vector3(RandomSize(), RandomSize(), RandomSize());
                float randomRotationY = Random.Range(0f, 360f);
                terrainObj.transform.rotation = Quaternion.Euler(0f, randomRotationY, 0f);
            }
        }


        private bool IsObstructed(Vector3 position)
        {
            Vector3 up = Vector3.up; // Raycast upwards to check for obstacles
            Vector3 referencePosition = new Vector3(position.x, navMesh.transform.position.y, position.z);
            if (Physics.Raycast(referencePosition, up, out RaycastHit hit, maxDistance))
            {
                return true; // Obstacle found
            }
            return false; // No obstacle found
        }

        private Vector3 GetRandomPositionOnNavMesh()
        {
            Vector3 randomPosition = Vector3.zero;
            var posToCheckObstruct = Vector3.zero;
            bool isGettingObjPos = false;
            while (!isGettingObjPos)
            {
                posToCheckObstruct = new Vector3(Random.Range(navMeshBounds.min.x, navMeshBounds.max.x), navMesh.transform.position.y, Random.Range(navMeshBounds.min.z, navMeshBounds.max.z));
               isGettingObjPos = !IsObstructed(posToCheckObstruct);
            }
            randomPosition = new Vector3(posToCheckObstruct.x, posToCheckObstruct.y + spawnOffset, posToCheckObstruct.z);
            return randomPosition;
        }
        private float RandomSize()
        {
            var size = Random.Range(sizeMin, sizeMax);
            return size;
        }
        public void SetPrefabArray()
        {
            var allObjects = Resources.LoadAll<GameObject>(folderName);
            prefabArr = allObjects;
        }
        public GameObject GetRandomPrefab()
        {
            var arrLength = prefabArr.Length;
            var randomPrefab = prefabArr[Random.Range(0, arrLength - 1)];
            return randomPrefab;
        }
    }
}



