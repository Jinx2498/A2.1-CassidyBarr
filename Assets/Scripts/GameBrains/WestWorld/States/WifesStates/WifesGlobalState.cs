using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.WifesStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/WifesGlobalState")]
    public class WifesGlobalState : State
    {
        State visitBathroomState; // cache state here to save multiple lookups
        State cookStewState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups
        MinersWifeEntity elsa; // // cache the miner's wife here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            visitBathroomState = StateManager.Lookup(typeof(VisitBathroom));
            cookStewState = StateManager.Lookup(typeof(CookStew));
            bob = EntityManager.Find<MinerEntity>("Bob");
            elsa = EntityManager.Find<MinersWifeEntity>("Elsa");
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            if (Random.value < 0.1f)
            {
                stateMachine.ChangeState(visitBathroomState);
            }
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            if (eventArguments.EventType == Events.HiHoneyImHome
                && eventArguments.ReceiverId == elsa.ID)
            {
                if (VerbosityDebug)
                {
                    Debug.Log($"Event {eventArguments.EventType} received by {elsa.name} at time: {Time.time}");
                }

                EventManager.Instance.Fire(Events.Message, elsa.ID, "Hi honey. Let me make you some of mah fine country stew");

                stateMachine.ChangeState(cookStewState);
                return true;
            }

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}