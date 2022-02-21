using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.MinersStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/EatStew")]
    public class EatStew : State
    {
        MinerEntity bob; // cache the miner here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            bob = EntityManager.Find<MinerEntity>("Bob");

            EventManager.Instance.Fire(Events.Message, bob.ID, "Smells Reaaal goood Elsa!");
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            EventManager.Instance.Fire(Events.Message, bob.ID, "Tastes real good too!");

            stateMachine.RevertToPreviousState();
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(
                Events.Message,
                bob.ID,
                "Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
        }
    }
}