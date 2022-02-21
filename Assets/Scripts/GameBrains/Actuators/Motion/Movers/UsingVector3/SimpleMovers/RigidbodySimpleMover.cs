using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers
{
    //[RequireComponent(typeof(Rigidbody))] // needs to attach to parent
    public sealed class RigidbodySimpleMover : SimpleMover
    {
        [SerializeField] bool useForce;

        Rigidbody rb;

        public override void Start()
        {
            base.Start();
            rb = GetComponentInParent<Rigidbody>();
        }

        protected override void CalculatePhysics(float deltaTime)
        {
            if (Speed < minimumSpeed) { return; }
            
            if (useForce)
            {
                throw new System.NotImplementedException(
                    "Homework: How can we use rb.AddForce to move properly?");
            }

            rb.velocity = Direction * Speed;
        }
    }
}