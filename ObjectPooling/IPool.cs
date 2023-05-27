
using UnityEngine;
using UnityEngine.Pool;



namespace RPG.Character
{
    public interface IPool 
    {
        public ObjectPool<GameObject> Pool { get; }
        public GameObject CreatePoolObject();
        public void OnDestroyPoolObject(GameObject gameObject);
        public void OnTakeFromPool(GameObject gameObject);
        public void OnReturnedToPool(GameObject gameObject);
    }

}

