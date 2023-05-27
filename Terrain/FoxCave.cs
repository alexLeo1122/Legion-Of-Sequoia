
using UnityEngine;

namespace RPG.Quest
{
    public class FoxCave : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DevilFox"))
            {
                other.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
}

