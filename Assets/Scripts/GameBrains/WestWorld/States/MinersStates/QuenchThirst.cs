using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.MinersStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/QuenchThirst")]
    public class QuenchThirst : State
    {
        State enterMineAndDigForNuggetState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            enterMineAndDigForNuggetState = StateManager.Lookup(typeof(EnterMineAndDigForNugget));
            bob = EntityManager.Find<MinerEntity>("Bob");

            if (bob.Location != Locations.Saloon)
            {
                EventManager.Instance.Fire(
                    Events.Message,
                    bob.ID,
                    "Boy, ah sure is thusty! Walking to the saloon");

                bob.Location = Locations.Saloon;
            }
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            bob.BuyAndDrinkAWhiskey();

            EventManager.Instance.Fire(Events.Message, bob.ID, "That's mighty fine sippin' liquer");

            stateMachine.ChangeState(enterMineAndDigForNuggetState);
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(Events.Message, bob.ID, "Leaving the saloon, feelin' good");
        }
    }
}