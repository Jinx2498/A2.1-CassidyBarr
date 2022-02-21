using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.WifesStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/VisitBathroom")]
    public class VisitBathroom : State
    {
        MinersWifeEntity elsa; // cache the miner's wife here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            elsa = EntityManager.Find<MinersWifeEntity>("Elsa");

            EventManager.Instance.Fire(
                Events.Message,
                elsa.ID,
                "Walkin' to the can. Need to powda mah pretty li'lle nose");
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            EventManager.Instance.Fire(Events.Message, elsa.ID, "Ahhhhhh! Sweet relief!");

            stateMachine.RevertToPreviousState();
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);

            EventManager.Instance.Fire(Events.Message, elsa.ID, "Leavin' the Jon");
        }
    }
}