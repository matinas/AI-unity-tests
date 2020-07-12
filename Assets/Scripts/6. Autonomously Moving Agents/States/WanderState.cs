using System;
using UnityEngine;

namespace AITests.AMA.States
{
    public class WanderState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;
        
        public WanderState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("Enter WanderState");

            controller.OnWanderCompleted += HandleWanderCompleted;
        }

        public override void Update()
        {
            controller.WanderFor(10.0f);
        }

        public override void Exit()
        {
            Debug.Log("Exit WanderState");

            controller.OnWanderCompleted -= HandleWanderCompleted;
        }

        private void HandleWanderCompleted()
        {
            OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.WANDER, newState = States.SEEK });
            Exit();
        }
    }
}