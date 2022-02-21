using GameBrains.Entities.V0;
using GameBrains.Percepts;
using UnityEngine;

namespace GameBrains.Sensors
{
    public class TargetEntitySensor : Sensor
    {
        [SerializeField] float sensorRange = 20.0f;
        public TargetEntity targetEntity;

        public override Percept Sense()
        {
            if (targetEntity != null)
            {
                var targetEntityPercept = new TargetEntityPercept();
                var agentPosition = Agent.transform.position;
                var shortestDistance = float.PositiveInfinity;

                var targetDistance = Vector3.Distance(agentPosition, targetEntity.transform.position);

                // Are we within range?
                if (targetDistance <= sensorRange && targetEntity != null)
                {
                    if (targetDistance < shortestDistance)
                    {
                        targetEntityPercept.targetEntity = targetEntity;
                        shortestDistance = targetDistance;
                        Debug.Log("New closest target entity. Position is: "
                                  + targetEntityPercept.targetEntity.transform.position);
                        return targetEntityPercept;
                    }
                }
            }

            return null;
        }
    }
}