using GameBrains.Entities.V1;
using GameBrains.Entities.V1.EntityManager;
using GameBrains.EventSystem;
using GameBrains.FiniteStateMachine;
using GameBrains.WestWorld.Entities;
using UnityEngine;

namespace GameBrains.WestWorld.States.WifesStates
{
    [CreateAssetMenu(menuName = "WestWorld/States/DoHouseWork")]
    public class DoHouseWork : State
    {
        MinersWifeEntity elsa; // cache the miner's wife here to save multiple lookups

        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);

            elsa = EntityManager.Find<MinersWifeEntity>("Elsa");

            EventManager.Instance.Fire(Events.Message, elsa.ID, "Time to do some more housework!");
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);

            switch (Random.Range(0, 3))
            {
                case 0:
                    EventManager.Instance.Fire(Events.Message, elsa.ID, "Moppin' the floor");
                    break;

                case 1:
                    EventManager.Instance.Fire(Events.Message, elsa.ID, "Washin' the dishes");
                    break;

                case 2:
                    EventManager.Instance.Fire(Events.Message, elsa.ID, "Makin' the bed");
                    break;
            }
        }
    }
}