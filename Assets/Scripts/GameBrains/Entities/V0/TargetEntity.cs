using UnityEngine;

namespace GameBrains.Entities.V0
{
    public class TargetEntity : Entity
    {
        void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject, 1f);
        }
    }
}