using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingTransformWrapper.Movers
{
    //[RequireComponent(typeof(CharacterController))]
    public sealed class CharacterControllerMover : Mover
    {
        [SerializeField] bool useSimpleMove;

        CharacterController characterController;

        public override void Start()
        {
            base.Start();
            characterController = GetComponentInParent<CharacterController>();
        }

        protected override void CalculatePhysics(float deltaTime)
        {
            // Use average of Vinitial and Vfinal
            // deltaP = (Vinital + Vfinal) / 2 * t
            // Vfinal = Vinitial + A * t
            // deltaP = (Vinitial + Vinitial + A * t) / 2 * t
            // deltaP = (2 * Vinitial + A * t) / 2 * t
            // deltaP = Vinitial * t + A * t * t / 2
            float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
            VectorXZ positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Velocity += Acceleration * deltaTime;

            if (useSimpleMove)
            {
                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                characterController.SimpleMove((Vector3)positionOffset / deltaTime);
            }
            else
            {
                // Type cast from VectorXZ to Vector3 sets Y to 0. Good.
                characterController.Move((Vector3)positionOffset);
                //TODO: Handle gravity.
            }
        }
    }
}