using GameBrains.Actuators.MotionData;
using GameBrains.EventSystem;
using GameBrains.Extensions.Attributes;
using GameBrains.Extensions.MonoBehaviours;
using GameBrains.FiniteStateMachine;
using GameBrains.GUI;
using GameBrains.Timers;
using UnityEngine;

namespace GameBrains.Entities.V1
{
	public class Entity : ExtendedMonoBehaviour
	{
		public int ID { get; protected set; }
		
		#region Player control

		[SerializeField] bool isPlayerControlled;

		public bool IsAiControlled
		{
			get => isPlayerControlled;

			set => isPlayerControlled = value;
		}

		#endregion
		
		#region Static Data
		
		[SerializeField] StaticData staticData;

		public StaticData StaticData
		{
			get => staticData;
			set => staticData = value;
		}
		
		#endregion Static Data
		
		#region Finite State Machine

		// Enable in Inspector before entering Playmode to configure the Finite State Machine
		[ReadOnlyInPlaymode] [SerializeField] protected bool usesFiniteStateMachine;

		public StateMachine StateMachine { get; protected set; }

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected State globalStartState;
		public State GlobalStartState { get => globalStartState; protected set => globalStartState = value; }

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected State startState;
		public State StartState { get => startState; protected set => startState = value; }

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected float smMinimumTimeMs;

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected  float smMaximumDelayMs;

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected RegulatorMode stateMachineMode;

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected DelayDistribution smDelayDistribution;

		[VisibleIf("usesFiniteStateMachine")]
		[ReadOnlyInPlaymode]
		[SerializeField] protected  AnimationCurve smDistributionCurve
			= new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

		#endregion Finite State Machine
		
		#region Event Message Viewer
		
		// Enable in Inspector before entering Playmode to configure the Finite State Machine
		[ReadOnlyInPlaymode] [SerializeField] protected bool usesEventMessageViewer;
		
		[VisibleIf("usesEventMessageViewer")]
		[ReadOnlyInPlaymode]
		[SerializeField] MessageViewer eventMessageViewer;
		
		public MessageViewer EventMessageViewer { get => eventMessageViewer; protected set => eventMessageViewer = value; }
		
		#endregion Event Message Viewer

		public override void Awake()
		{
			base.Awake();
			
			ID = EntityManager.EntityManager.NextID;
			EntityManager.EntityManager.Add(this);

			StaticData = transform;
			
			if (usesFiniteStateMachine) SetupFiniteStateMachine();
		}
		
		protected virtual void SetupFiniteStateMachine()
		{
			StateMachine = gameObject.AddComponent<StateMachine>();
			StateMachine.MinimumTimeMs = smMinimumTimeMs;
			StateMachine.MaximumDelayMs = smMaximumDelayMs;
			StateMachine.Mode = stateMachineMode;
			StateMachine.DelayDistribution = smDelayDistribution;
			StateMachine.DistributionCurve = smDistributionCurve;
			if (StartState != null) StateMachine.ChangeState(StartState);
			if (GlobalStartState != null) StateMachine.ChangeGlobalState(GlobalStartState);
		}
		
		#region Event Handling

		// Typical usages:
		// EventManager.Instance.Enqueue(Events.MyEntityEvent, receiver.ID, data);
		// EventManager.Instance.Fire(Events.MyEntityEvent, receiver.ID, data);
		// EventManager.Instance.Fire(Events.Message, lookedUpReceiver.ID, message);
		// EventManager.Instance.Enqueue(Events.Message, delay, receiver.ID, message);
		// EventManager.Instance.Subscribe<type>(Events.MyEvent, HandleEvent);

		public virtual bool HandleEvent<T>(Event<T> eventArguments)
		{
			if (eventArguments.EventType == Events.Message
			    && eventArguments.ReceiverId == ID)
			{
				if (usesEventMessageViewer)
				{
					string message = eventArguments.EventData as string;
					EventMessageViewer.messageQueue.AddMessage(message);
				}
				else
				{
					Debug.Log("Entity " + name + eventArguments);
				}
				
				return true;
			}

			if (eventArguments.EventType == Events.MyEntityEvent
			    && eventArguments.ReceiverId == ID)
			{
				Debug.Log("Entity MyEvent " + name + eventArguments);
				return true;
			}
			
			if (usesFiniteStateMachine)
			{
				return FiniteStateMachineHandleEvent(eventArguments);
			}

			return false;
		}
		
		protected virtual bool FiniteStateMachineHandleEvent<TEvent>(Event<TEvent> eventArguments)
		{
			return StateMachine != null && StateMachine.HandleEvent(eventArguments);
		}
		
		#endregion Event Handling
	}
}