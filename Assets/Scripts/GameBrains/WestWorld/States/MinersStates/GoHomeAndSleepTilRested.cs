using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.MinersStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/GoHomeAndSleepTilRested")]
    public class GoHomeAndSleepTilRested : State
    {
        State enterMineAndDigForNuggetState; // cache state here to save multiple lookups
        State eatStewState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups
        MinersWifeEntity elsa; // cache the miner's wife here to save multiple lookups
        
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            enterMineAndDigForNuggetState = StateManager.Lookup(typeof(EnterMineAndDigForNugget));
            eatStewState = StateManager.Lookup(typeof(EatStew));
            bob = EntityManager.Find<MinerEntity>("Bob");
            elsa = EntityManager.Find<MinersWifeEntity>("Elsa");
            
            if (bob.Location != Locations.Shack)
            {
                EventManager.Instance.Fire(Events.Message, bob.ID, "Walkin' home");

                bob.Location = Locations.Shack;

                // let the wife know I'm home
                EventManager.Instance.Fire(Events.HiHoneyImHome, bob.ID, elsa.ID, string.Empty);
            }
        }
        
        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            // if miner is not fatigued start to dig for nuggets again.
            if (!bob.IsFatigued)
            {
                EventManager.Instance.Fire(
                    Events.Message,
                    bob.ID,
                    "All mah fatigue has drained away. Time to find more gold!");

                stateMachine.ChangeState(enterMineAndDigForNuggetState);
            }
            else
            {
                // sleep
                bob.DecreaseFatigue();

                EventManager.Instance.Fire(Events.Message, bob.ID, "ZZZZ... ");
            }
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            if (eventArguments.EventType == Events.StewReady
                && eventArguments.ReceiverId == bob.ID)
            {
                if (VerbosityDebug)
                {
                    Debug.Log($"Event {eventArguments.EventType} received by {bob.name} at time: {Time.time}");
                }

                EventManager.Instance.Fire(Events.Message, bob.ID, "Okay Hun, ahm a comin'!");

                stateMachine.ChangeState(eatStewState);
                return true;
            }

            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}