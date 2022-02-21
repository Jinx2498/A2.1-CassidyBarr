using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector2.Movers
{
    //[RequireComponent(typeof(CharacterController))] // needs to attach to parent
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
            Vector2 positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Velocity += Acceleration * deltaTime;

            if (useSimpleMove)
            {
                // XY -> XYZ! Not what we want
                characterController.SimpleMove(positionOffset / deltaTime);

                // This works but is inelegant. Need a VectorXZ type.
                // var positionOffsetXYZ = new Vector3(positionOffset.x, 0, positionOffset.y);
                // characterController.SimpleMove(positionOffsetXYZ / deltaTime);
            }
            else
            {
                // XY -> XYZ! Not what we want
                characterController.Move(positionOffset);

                // This works but is inelegant. Need a VectorXZ type.
                // var positionOffsetXYZ = new Vector3(positionOffset.x, 0, positionOffset.y);
                // characterController.Move(positionOffsetXYZ);

                //TODO: Handle gravity.
            }
        }
    }
}