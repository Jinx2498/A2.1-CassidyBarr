using GameBrains.Actuators.MotionData;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class RigidbodyMotor : Motor
    {
        Rigidbody rb;

        public override void Start()
        {
            base.Start();
            rb = GetComponentInParent<Rigidbody>();
        }

        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime, false);
            rb.velocity = (Vector3)kinematicData.Velocity;
        }
    }
}