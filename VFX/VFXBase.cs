using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace RPG.VFX
{
    public abstract class VFXBase: MonoBehaviour
    {
        [SerializeField] private float timeToDisactive = 1.5f;
        public ObjectPool<GameObject> Pool { get; set; }

        private void OnEnable()
        {
            StartCoroutine(DisactiveAfterTime(timeToDisactive));
        }
        public virtual IEnumerator  DisactiveAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            Pool.Release(this.gameObject);
        }
    }
}


