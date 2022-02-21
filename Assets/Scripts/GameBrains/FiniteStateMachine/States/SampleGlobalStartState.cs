using GameBrains.EventSystem;
using UnityEngine;

namespace GameBrains.FiniteStateMachine.States
{
    [CreateAssetMenu(menuName = "StateMachine/States/SampleGlobalStartState")]
    public class SampleGlobalStartState : State
    {
        public override void Enter(StateMachine stateMachine)
        {
            base.Enter(stateMachine);
        }

        public override void Execute(StateMachine stateMachine)
        {
            base.Execute(stateMachine);
        }

        public override void Exit(StateMachine stateMachine)
        {
            base.Exit(stateMachine);
        }

        public override bool HandleEvent<TEvent>(
            StateMachine stateMachine,
            Event<TEvent> eventArguments)
        {
            return base.HandleEvent(stateMachine, eventArguments);
        }
    }
}