using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector2.SimpleMovers
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

            // XY -> XYZ! Not what we want
            rb.velocity = Direction * Speed;

            // This works but is inelegant. Need a VectorXZ type.
            // var directionXYZ = new Vector3(Direction.x, 0, Direction.y);
            // rb.velocity = directionXYZ * Speed;
        }
    }
}