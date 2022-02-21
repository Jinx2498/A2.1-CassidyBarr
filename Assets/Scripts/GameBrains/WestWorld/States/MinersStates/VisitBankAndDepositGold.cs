using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.MinersStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/VisitBankAndDepositGold")]
    public class VisitBankAndDepositGold : State
    {
        State goHomeAndSleepTilRestedState; // cache state here to save multiple lookups
        State enterMineAndDigForNuggetState; // cache state here to save multiple lookups
        MinerEntity bob; // cache the miner here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            goHomeAndSleepTilRestedState = StateManager.Lookup(typeof(GoHomeAndSleepTilRested));
            enterMineAndDigForNuggetState = StateManager.Lookup(typeof(EnterMineAndDigForNugget));
            bob = EntityManager.Find<MinerEntity>("Bob");

            if (bob.Location != Locations.Bank)
            {
                EventManager.Instance.Fire(Events.Message, bob.ID, "Goin' to the bank. Yes siree");

                bob.Location = Locations.Bank;
            }
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            // deposit the gold
            bob.AddToMoneyInBank(bob.GoldCarried);
            bob.GoldCarried = 0;
            EventManager.Instance.Fire(
                Events.Message,
                bob.ID,
                $"Depositing gold. Total savings now: {bob.MoneyInBank}");

            // wealthy enough to have a well earned rest?
            if (bob.MoneyInBank >= bob.comfortLevel)
            {
                EventManager.Instance.Fire(
                    Events.Message,
                    bob.ID,
                    "WooHoo! Rich enough for now. Back home to mah li'lle lady");
                stateMachine.ChangeState(goHomeAndSleepTilRestedState);
            }

            // otherwise get more gold
            else
            {
                stateMachine.ChangeState(enterMineAndDigForNuggetState);
            }
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(Events.Message, bob.ID, "Leavin' the bank");
        }
    }
}