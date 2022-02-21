using System.Collections.Generic;
using GameBrains.Actions;
using GameBrains.Entities.V0;
using GameBrains.Percepts;
using UnityEngine;

namespace GameBrains.Minds
{
	public class SeekerMind : Mind
	{
		#region Target Types
		
		public enum TargetTypes
		{
			None,
			First,
			Closest,
			Valued,
			Random
		}
		
		[SerializeField] TargetTypes targetType;
		public TargetTypes TargetType => targetType;

		#endregion Target Types
		
		// TODO: Make parameters settable through Mind decisions??
		
		// How close is close enough?
		[SerializeField] protected float desiredSatisfactionRadius = 0.5f; // TODO: Should depend on Agent radius??
		// How fast should we move?
		[SerializeField] protected float desiredSpeed = 100f; // TODO: Should depend on Actuator capabilities??
		[SerializeField] protected float moveTimeToLive = 5f;

		[SerializeField] protected float desiredSatisfactionAngle = 2; // TODO: Should depend on Actuator capabilities??
		[SerializeField] protected float desiredAngularSpeed = 1f; // TODO: Should depend on Actuator capabilities??
		[SerializeField] protected float turnTimeToLive = 5f;
		
		public override List<Action> Think(IEnumerable<Percept> percepts)
		{
			var actions = new List<Action>();
			Transform agentTransform = Agent.transform;
			Vector3 agentPosition = agentTransform.position;

			var targetEntity = ChooseTargetEntity(percepts);

			Vector3 targetPosition = 
				targetEntity != null ? targetEntity.transform.position : ChooseTargetPosition(percepts);

			// TODO: If Actuator cannot achieve desire this keeps trying endlessly??
			if (Vector3.Distance(agentPosition, targetPosition) > desiredSatisfactionRadius)
			{
				var changeSpeedAction
					= new ChangeSpeedAction
					{
						desiredSpeed = desiredSpeed,
						completionStatus = Action.CompletionsStates.InProgress,
						timeToLive = moveTimeToLive
					};
				actions.Add(changeSpeedAction);
			}
			else
			{
				var changeSpeedAction
					= new ChangeSpeedAction
					{
						desiredSpeed = 0,
						completionStatus = Action.CompletionsStates.InProgress,
						timeToLive = moveTimeToLive
					};
				actions.Add(changeSpeedAction);
			}

			Vector3 desiredDirection = (targetPosition - agentPosition).normalized;
			if (desiredDirection.magnitude > 0)
			{
				var changeDirectionAction
					= new ChangeDirectionAction
					{
						desiredDirection = desiredDirection,
						completionStatus = Action.CompletionsStates.InProgress,
						timeToLive = turnTimeToLive
					};
				actions.Add(changeDirectionAction);
			}

			return actions;
		}
		
		#region Choose Target Entity
		
		protected TargetEntity ChooseTargetEntity(IEnumerable<Percept> percepts)
		{
			switch (TargetType)
			{
				// TODO: prioritize targets
				case TargetTypes.None:
					return null;
				case TargetTypes.First:
					return ChooseFirstTargetEntity(percepts);
				case TargetTypes.Closest:
					return ChooseClosestTargetEntity(percepts);
				case TargetTypes.Valued:
					Debug.LogWarning("TargetType.Valued is not implemented. Defaulting to Random.");
					return ChooseRandomTargetEntity(percepts);
				default:
					return ChooseRandomTargetEntity(percepts);
			}
		}
		
		protected TargetEntity ChooseRandomTargetEntity(IEnumerable<Percept> percepts)
		{
			var targetEntities = new List<TargetEntity>();

			foreach (Percept percept in percepts)
			{
				if (percept is TargetEntityPercept targetEntityPercept)
				{
					targetEntities.Add(targetEntityPercept.targetEntity);
				}
			}

			if (targetEntities.Count > 0)
			{
				var randomIndex = Random.Range(0, targetEntities.Count);

				print("LookAt = " + randomIndex);

				return targetEntities[randomIndex];
			}

			return null; // no target
		}
		
		protected TargetEntity ChooseFirstTargetEntity(IEnumerable<Percept> percepts)
		{
			foreach (Percept percept in percepts)
			{
				if (percept is TargetEntityPercept targetEntityPercept)
				{
					return targetEntityPercept.targetEntity;
				}
			}

			return null; // no target
		}
		
		protected TargetEntity ChooseClosestTargetEntity(IEnumerable<Percept> percepts)
		{
			var targetEntities = new List<TargetEntity>();

			foreach (Percept percept in percepts)
			{
				if (percept is TargetEntityPercept targetEntityPercept)
				{
					targetEntities.Add(targetEntityPercept.targetEntity);
				}
			}

			int closestIndex = -1;
			float closestDistance = float.PositiveInfinity;

			for (int i = 0; i < targetEntities.Count; i++)
			{
				float distance
					= Vector3.Distance(Agent.transform.position, targetEntities[i].transform.position);

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestIndex = i;
				}
			}

			if (closestIndex != -1)
			{
				return targetEntities[closestIndex];
			}

			return null; // no target
		}
		
		#endregion Choose Target Entity
		
		#region Choose Target Position

		protected Vector3 ChooseTargetPosition(IEnumerable<Percept> percepts)
		{
			switch (TargetType)
			{
				// TODO: prioritize targets
				case TargetTypes.None:
					return Agent.transform.position;
				case TargetTypes.First:
					return ChooseFirstTargetPosition(percepts);
				case TargetTypes.Closest:
					return ChooseClosestTargetPosition(percepts);
				case TargetTypes.Valued:
					Debug.LogWarning("TargetType.Valued is not implemented. Defaulting to Random.");
					return ChooseRandomTargetPosition(percepts);
				default:
					return ChooseRandomTargetPosition(percepts);
			}
		}

		protected Vector3 ChooseRandomTargetPosition(IEnumerable<Percept> percepts)
		{
			List<Vector3> targetPositions = new List<Vector3>();

			foreach (Percept percept in percepts)
			{
				if (percept is PositionPercept positionPercept
				    && positionPercept.position.HasValue)
				{
					targetPositions.Add(positionPercept.position.Value);
				}
			}

			if (targetPositions.Count > 0)
			{
				var randomIndex = Random.Range(0, targetPositions.Count);

				print("LookAt = " + randomIndex);

				return targetPositions[randomIndex];
			}
			
			return Agent.transform.position; // no target
		}

		protected Vector3 ChooseFirstTargetPosition(IEnumerable<Percept> percepts)
		{
			foreach (Percept percept in percepts)
			{
				if (percept is PositionPercept positionPercept
					&& positionPercept.position.HasValue)
				{
					return positionPercept.position.Value;
				}
			}

			return Agent.transform.position; // no target
		}
		
		protected Vector3 ChooseClosestTargetPosition(IEnumerable<Percept> percepts)
		{
			List<Vector3> targetPositions = new List<Vector3>();

			foreach (Percept percept in percepts)
			{
				if (percept is PositionPercept positionPercept
				    && positionPercept.position.HasValue)
				{
					targetPositions.Add(positionPercept.position.Value);
				}
			}

			int closestIndex = -1;
			float closestDistance = float.PositiveInfinity;

			for (int i = 0; i < targetPositions.Count; i++)
			{
				float distance 
					= Vector3.Distance(Agent.transform.position, targetPositions[i]);

				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestIndex = i;
				}
			}

			if (closestIndex != -1)
			{
				return targetPositions[closestIndex];
			}

			return Agent.transform.position; // no target
		}
		
		#endregion Choose Target Position
	}
}