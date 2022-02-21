using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.WifesStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/CookStew")]
    public class CookStew : State
    {
        State doHouseWorkState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups
        MinersWifeEntity elsa;

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            doHouseWorkState = StateManager.Lookup(typeof(DoHouseWork));
            bob = EntityManager.Find<MinerEntity>("Bob");
            elsa = EntityManager.Find<MinersWifeEntity>("Elsa");

            if (!elsa.IsCooking)
            {
                EventManager.Instance.Fire(Events.Message, elsa.ID, "Putting the stew in the oven.");

                // send a delayed message to myself so that I know when to take the stew out of the oven
                EventManager.Instance.Enqueue(Events.StewReady, 4.0f, elsa.ID, elsa.ID);

                elsa.IsCooking = true;
            }
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            EventManager.Instance.Fire(Events.Message, elsa.ID, "Fussin' over food");
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(Events.Message, elsa.ID, "Puttin' the stew on the table");

        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            if (eventArguments.EventType == Events.StewReady
                && eventArguments.ReceiverId == elsa.ID)
            {
                if (VerbosityDebug)
                {
                    Debug.Log($"Event {eventArguments.EventType} received by {elsa.name} at time: {Time.time}");
                }

                EventManager.Instance.Fire(Events.Message, elsa.ID, "StewReady! Lets eat");

                // let hubby know the stew is ready
                EventManager.Instance.Fire(Events.StewReady, elsa.ID, bob.ID, string.Empty);

                elsa.IsCooking = false;

                stateMachine.ChangeState(doHouseWorkState);
                return true;
            }

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}