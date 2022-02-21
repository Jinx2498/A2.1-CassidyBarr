using GameBrains.Actuators.MotionData;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace Testing
{
    [AddComponentMenu("Scripts/Testing/W1 Test Static Data")]
    public class W11TestStaticData : ExtendedMonoBehaviour
    {
        public StaticData staticData;
        public Transform agentTransform;
        public VectorXYZ lookTargetPosition;
        public VectorXYZ moveTargetPosition;
        public bool checkHasLineOfSight;
        public bool checkCanMoveTo;
        public bool checkCanStepLeft;
        public bool checkCanStepRight;
        public bool checkIsAtPosition;

        public RayCastVisualizer rayCastVisualizer;
        public CapsuleCastVisualizer capsuleCastVisualizer;

        public float castRadiusMultiplier = 1.0f;
        public float closeEnoughDistance = 1.0f;

        public override void Awake()
        {
            base.Awake();
            
            staticData
                //= new StaticData(agentTransform) { Radius = 0.75f }; // weebles with arms are wider
                = new StaticData(agentTransform);
            rayCastVisualizer = ScriptableObject.CreateInstance<RayCastVisualizer>();
            capsuleCastVisualizer = ScriptableObject.CreateInstance<CapsuleCastVisualizer>();
        }

        public override void Update()
        {
            base.Update();
            
            if (checkHasLineOfSight)
            {
                checkHasLineOfSight = false;

                Debug.Log(
                    staticData.HasLineOfSight(
                        lookTargetPosition,
                        rayCastVisualizer,
                        true));
            }

            if (checkCanMoveTo)
            {
                checkCanMoveTo = false;

                Debug.Log(
                    staticData.CanMoveTo(
                        moveTargetPosition,
                        capsuleCastVisualizer,
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanStepLeft)
            {
                checkCanStepLeft = false;

                Debug.Log(
                    staticData.CanStepLeft(
                        capsuleCastVisualizer, 
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanStepRight)
            {
                checkCanStepRight = false;

                Debug.Log(
                    staticData.CanStepRight(
                        capsuleCastVisualizer, 
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkCanMoveTo)
            {
                checkCanMoveTo = false;

                Debug.Log(
                    staticData.CanMoveTo(
                        moveTargetPosition,
                        capsuleCastVisualizer,
                        true,
                        false,
                        castRadiusMultiplier));
            }

            if (checkIsAtPosition)
            {
                checkIsAtPosition = false;

                Debug.Log(
                    staticData.IsAtPosition(
                        staticData.Position + VectorXYZ.forward, 
                        closeEnoughDistance));
            }
        }
    }
}