using GameBrains.Extensions.MathExtensions;
using GameBrains.Extensions.Vectors;
using UnityEngine;

namespace GameBrains.Actuators.MotionData
{
    [System.Serializable]
    public class KinematicData : StaticData
    {
        #region Constructor

        public KinematicData(Transform t) : base(t)
        {
            MaximumSpeed = DefaultMaximumSpeed;
            MaximumAngularSpeed = DefaultMaximumAngularSpeed;
            MaximumAcceleration = DefaultMaximumAcceleration;
            MaximumAngularAcceleration = DefaultMaximumAngularAcceleration;

            Velocity = VectorXZ.zero;
            AngularVelocity = 0;

            Acceleration = VectorXZ.zero;
            AngularAcceleration = 0;
        }

        #endregion Constructor

        #region Copy constructor

        public KinematicData(KinematicData kinematicDataSource)
            : base(kinematicDataSource)
        {
            MaximumSpeed = kinematicDataSource.MaximumSpeed;
            MaximumAngularSpeed = kinematicDataSource.MaximumAngularSpeed;
            MaximumAcceleration = kinematicDataSource.MaximumAcceleration;
            MaximumAngularAcceleration = kinematicDataSource.MaximumAngularAcceleration;

            Velocity = kinematicDataSource.Velocity;
            AngularVelocity = kinematicDataSource.AngularVelocity;

            Acceleration = kinematicDataSource.Acceleration;
            AngularAcceleration = kinematicDataSource.AngularAcceleration;
        }

        #endregion Copy constructor

        #region Kinematic Data

        protected VectorXZ velocity;
        public VectorXZ Velocity
        {
            get => velocity;

            set => velocity = Math.LimitMagnitude(value, MaximumSpeed);
        }

        protected float angularVelocity;
        public float AngularVelocity
        {
            get => angularVelocity;

            set => angularVelocity = Math.LimitMagnitude(value, MaximumAngularSpeed);
        }

        protected VectorXZ acceleration;
        public VectorXZ Acceleration
        {
            get => acceleration;

            set => acceleration = Math.LimitMagnitude(value, MaximumAcceleration);
        }

        protected float angularAcceleration;
        public float AngularAcceleration
        {
            get => angularAcceleration;

            set => angularAcceleration = Math.LimitMagnitude(value, MaximumAngularAcceleration);
        }

        public float Speed => velocity.magnitude;

        public VectorXYZ VelocityXYZ => (VectorXYZ)Velocity;

        #endregion Kinematic Data

        #region Default limits

        public const float DefaultMaximumSpeed = 5;
        public float MaximumSpeed { get; set; }

        public const float DefaultMaximumAngularSpeed = 360;
        public float MaximumAngularSpeed { get; set; }

        public const float DefaultMaximumAcceleration = 0.5f;
        public float MaximumAcceleration { get; set; }

        public const float DefaultMaximumAngularAcceleration = 180;
        public float MaximumAngularAcceleration { get; set; }

        #endregion

        #region Update

        public virtual void Update(float deltaTime)
        {
            CalculatePosition(deltaTime);
            CalculateOrientation(deltaTime);
            
            CalculateVelocity(deltaTime);
            CalculateAngularVelocity(deltaTime);
        }
        
        public virtual void DoUpdate(float deltaTime, bool updatePosition = true)
        {
            if (updatePosition) CalculatePosition(deltaTime);

            CalculateOrientation(deltaTime);

            UpdateVelocities(deltaTime);
        }
        
        public void CalculatePosition(float deltaTime)
        {
            
            // Use average of Vinitial and Vfinal
            // deltaP = (Vinital + Vfinal) / 2 * t
            // Vfinal = Vinitial + A * t
            // deltaP = (Vinitial + Vinitial + A * t) / 2 * t
            // deltaP = (2 * Vinitial + A * t) / 2 * t
            // deltaP = Vinitial * t + A * t * t / 2
            float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
            VectorXZ positionOffset = (Velocity * deltaTime) + (Acceleration * halfDeltaTimeSquared);
            Location += positionOffset;
        }

        public void CalculateOrientation(float deltaTime)
        {
            // Use average of AVinitial and AVfinal
            // deltaO = (AVinital + AVfinal) / 2 * t
            // AVfinal = AVinitial + AA * t
            // deltaO = (AVinitial + AVinitial + AA * t) / 2 * t
            // deltaO = (2 * AVinitial + AA * t) / 2 * t
            // deltaO = AVinitial * t + AA * t * t / 2
            float halfDeltaTimeSquared = (deltaTime * deltaTime) / 2;
            float orientationOffset = (AngularVelocity * deltaTime) + (AngularAcceleration * halfDeltaTimeSquared);
            Orientation += orientationOffset;
        }
        
        public void UpdateVelocities(float deltaTime)
        {
            CalculateVelocity(deltaTime);
            CalculateAngularVelocity(deltaTime);
        }

        public void CalculateVelocity(float deltaTime)
        {
            Velocity += Acceleration * deltaTime;
        }

        public void CalculateAngularVelocity(float deltaTime)
        {
            AngularVelocity += AngularAcceleration * deltaTime;
        }

        #endregion
    }
}