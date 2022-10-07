using UnityEngine;

namespace FGWorms.Gameplay
{
    public class KillZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Health>(out var health))
            {
                health.DealDamage(999);
            }
        }
    }
}

