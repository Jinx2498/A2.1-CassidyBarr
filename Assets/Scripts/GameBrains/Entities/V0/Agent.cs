using System.Collections.Generic;
using GameBrains.Actions;
using GameBrains.Actuators;
using GameBrains.Minds;
using GameBrains.Percepts;
using GameBrains.PerformanceMeasures;
using GameBrains.Sensors;
using UnityEngine;

namespace GameBrains.Entities.V0
{
    public class Agent : Entity
    {
        #region Sensors
        
        [SerializeField] protected List<Sensor> sensors = new List<Sensor>();
		
        // TODO: Make protected and add public accessors
        public virtual List<Sensor> Sensors
        {
            get => sensors;
            set => sensors = value;
        }
        
        #endregion Sensors

        #region Minds
        
        [SerializeField] protected Mind mind;
        
        // TODO: Make protected and add public accessors
        public virtual Mind Mind
        {
            get => mind;
            set => mind = value;
        }
        
        #endregion Minds

        #region Actuators

        [SerializeField] protected List<Actuator> actuators = new List<Actuator>();
        
        // TODO: Make protected and add public accessors
        public virtual List<Actuator> Actuators
        {
            get => actuators;
            set => actuators = value;
        }

        #endregion Actuators

        #region Performance Measures
        
        [SerializeField] protected PerformanceMeasure performanceMeasure;
        
        // TODO: Make protected and add public accessors
        public virtual PerformanceMeasure PerformanceMeasure
        {
            get => performanceMeasure;
            set => performanceMeasure = value;
        }
        
        #endregion Performance Measures
        
        #region Percepts
        
        protected List<Percept> currentPercepts;
        
        #endregion Percepts
        
        #region Target

        public Transform targetTransform;

        #endregion Target
        
        #region Think Types
        
        public enum ThinkTypes
        {
            Replace,
            Add,
            Merge
        }

        [SerializeField] ThinkTypes thinkTypes;
        public ThinkTypes ThinkType => thinkTypes;
        
        #endregion Think Types
        
        #region Motor Types
        
        public enum MotorTypes
        {
            None,
            TransformTranslate,
            CharacterController,
            Rigidbody
        }
        
        [SerializeField] MotorTypes motorType;
        public MotorTypes MotorType => motorType;
        
        #endregion Motor Types

        #region Actions

        // TODO: Modify to continue or interrupt action currently in progress
        protected List<Action> currentActions = new List<Action>();

        #endregion
        
        public override void Awake()
        {
            base.Awake();

            switch (MotorType)
            {
                case MotorTypes.CharacterController:
                    SetupCharacterController();
                    break;
                case MotorTypes.Rigidbody:
                    SetupRigidbody();
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
            
            Sense(Time.deltaTime);

            Think(Time.deltaTime);

            Act(Time.deltaTime);
        }
        
        #region Sense, think, and act

        protected virtual void Sense(float deltaTime)
        {
            currentPercepts = new List<Percept>();
            
            foreach (Sensor sensor in Sensors)
            {
                if (sensor.SensorUpdateRegulator.IsReady)
                {
                    var currentPercept = sensor.Sense();
                    if (currentPercept != null)
                    {
                        currentPercepts.Add(currentPercept);
                    }
                }
            }
        }

        protected virtual void Think(float deltaTime)
        {
            if (Mind != null && Mind.MindUpdateRegulator.IsReady)
            {
                // TODO: Should we deal with inprogress actions or just drop them
                ChooseThinkType(currentPercepts);

                print("Action count = " + currentActions.Count);
            }
        }

        protected virtual void Act(float deltaTime)
        {
            foreach (Actuator actuator in Actuators)
            {
                if (actuator.ActuatorUpdateRegulator.IsReady)
                {
                    actuator.Act(currentActions);

                    CheckStatus();
                }
            }
        }

        #endregion Sense, think, and act
        
        protected void ChooseThinkType(List<Percept> percepts)
        {
            if (ThinkType == ThinkTypes.Replace)
            {
                currentActions = Mind.Think(percepts);
            }
            else if (ThinkType == ThinkTypes.Add)
            {
                currentActions.AddRange(Mind.Think(percepts));
            }
            else if (ThinkType == ThinkTypes.Merge)
            {
                MergeActions(Mind.Think(percepts));
            }
            else
            {
                Debug.LogWarning("Unsupported ThinkType");
            }
        }
        
        protected void MergeActions(List<Action> newActions)
        {
            foreach (Action action in newActions)
            {
                bool added = false;
                for (int i = 0; i < currentActions.Count; i++)
                {
                    // TODO: Can we have different actions of the same type??
                    if (currentActions[i].GetType() == action.GetType())
                    {
                        print("Action Interrupted: " + currentActions[i]);
                        currentActions[i] = action; // replace
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    currentActions.Add(action);
                }
            }
        }
        
        protected void CheckStatus()
        {
            for (int i = 0; i < currentActions.Count; i++)
            {
                if (currentActions[i].completionStatus == Action.CompletionsStates.Complete)
                {
                    print("Action Completed: " + currentActions[i]);
                    currentActions.RemoveAt(i);
                    i--;
                }
                else
                {
                    currentActions[i].timeToLive -= Time.deltaTime;
                    if (currentActions[i].timeToLive <= 0)
                    {
                        print("Action Timed Out: " + currentActions[i]);
                        currentActions[i].completionStatus = Action.CompletionsStates.Failed;
                        // Let failed remove this action
                    }

                    if (currentActions[i].completionStatus == Action.CompletionsStates.Failed)
                    {
                        print("Action Failed: " + currentActions[i]);
                        currentActions.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        
        protected void SetupCharacterController()
        {
            if (gameObject.GetComponent<CharacterController>() != null) { return; }

            var characterController = gameObject.AddComponent<CharacterController>();
            Vector3 center = characterController.center;
            center.y = 1.08f; // Agent's pivot is at 0, not its center
            characterController.center = center;
        }
        
        protected void SetupRigidbody()
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                //rb.isKinematic = true;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
			
            var capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

            if (capsuleCollider == null)
            {
                capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                Vector3 center = capsuleCollider.center;
                center.y = 1.08f; // Agent's pivot is at 0, not its center
                capsuleCollider.center = center;
                capsuleCollider.height = 2;
            }
        }
    }
}