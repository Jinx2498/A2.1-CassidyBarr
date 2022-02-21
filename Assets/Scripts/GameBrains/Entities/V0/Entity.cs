using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Entities.V0
{
    public class Entity : ExtendedMonoBehaviour
    {
        public override void Update()
        {
            if (!Application.IsPlaying(this)) return;
        }
    }
}