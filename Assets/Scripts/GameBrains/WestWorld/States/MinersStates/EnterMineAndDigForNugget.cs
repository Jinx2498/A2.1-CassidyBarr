using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.MinersStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/DoHouseWork")]
    public class EnterMineAndDigForNugget : State
    {
        State visitBankAndDepositGoldState; // cache state here to save multiple lookups
        State quenchThirstState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            visitBankAndDepositGoldState = StateManager.Lookup(typeof(VisitBankAndDepositGold));
            quenchThirstState = StateManager.Lookup(typeof(QuenchThirst));

            bob = EntityManager.Find<MinerEntity>("Bob");

            if (bob.Location != Locations.Goldmine)
            {
                EventManager.Instance.Fire(Events.Message, bob.ID, "Walkin' to the goldmine");

                bob.Location = Locations.Goldmine;
            }
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            // Now the miner is at the goldmine, he digs for gold until he
            // is carrying in excess of MaxNuggets. If he gets thirsty during
            // his digging, he packs up work for a while and changes state to
            // go to the saloon for a whiskey.
            bob.AddToGoldCarried(1);
            bob.IncreaseFatigue();
            bob.IncreaseThirst();
            EventManager.Instance.Fire(Events.Message, bob.ID, "Pickin' up a nugget");

            // if enough gold mined, go and put it in the bank
            if (bob.ArePocketsFull)
            {
                stateMachine.ChangeState(visitBankAndDepositGoldState);
            }

            // if thirsty, go to the saloon
            if (bob.IsThirsty)
            {
                stateMachine.ChangeState(quenchThirstState);
            }
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(
                Events.Message,
                bob.ID,
                "Ah'm leavin' the goldmine with mah pockets full o' sweet gold");
        }
    }
}