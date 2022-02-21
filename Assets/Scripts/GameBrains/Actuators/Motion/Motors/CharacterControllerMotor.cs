using GameBrains.Actuators.MotionData;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Motors
{
    public sealed class CharacterControllerMotor : Motor
    {
        CharacterController characterController;

        public override void Start()
        {
            base.Start();
            characterController = GetComponentInParent<CharacterController>();
        }

        public override void CalculatePhysics(KinematicData kinematicData, float deltaTime)
        {
            kinematicData.DoUpdate(deltaTime, false);
            characterController.SimpleMove((Vector3)kinematicData.Velocity);
        }
    }
}