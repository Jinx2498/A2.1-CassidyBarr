using UnityEngine;

namespace GameBrains.Actuators.Motion.Movers.UsingVector3.SimpleMovers
{
    //[RequireComponent(typeof(CharacterController))] // needs to attach to parent
    public sealed class CharacterControllerSimpleMover : SimpleMover
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
            if (Speed < minimumSpeed) { return; }
            
            Vector3 positionOffset = Direction * Speed;
            if (useSimpleMove)
            {
                characterController.SimpleMove(positionOffset);
            }
            else
            {
                characterController.Move(positionOffset * deltaTime);
                //TODO: Handle gravity
            }
        }
    }
}