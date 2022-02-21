using GameBrains.Extensions.MonoBehaviours;
using UnityEngine;

namespace GameBrains.Weebles
{
    public class MoveInCircles : ExtendedMonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] float angularSpeed = -1;

        public override void Update()
        {
            base.Update();
            
            transform.position += transform.forward * (speed * Time.deltaTime);
            transform.Rotate(0f, angularSpeed * Time.deltaTime, 0f);
        }
    }
}