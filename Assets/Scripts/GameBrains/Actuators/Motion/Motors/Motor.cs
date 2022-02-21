using GameBrains.Actuators.MotionData;

namespace GameBrains.Actuators.Motion.Motors
{
    public abstract class Motor : Actuator
    {
        // movement algorithm updates kinematic data
        public abstract void CalculatePhysics(KinematicData kinematicData, float deltaTime);
    }
}