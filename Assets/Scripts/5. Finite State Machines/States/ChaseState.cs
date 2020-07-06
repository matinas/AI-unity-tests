using System;
using UnityEngine;

namespace AITests.FSM.States
{
    public class ChaseState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;

        public ChaseState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("ChaseState - Enter");

            controller.OnChasePlayerCompleted += HandleOnChasePlayerCompleted;
        }

        public override void Update()
        {
            Debug.Log("ChaseState - Update");

            controller.ChasePlayer(10.0f);
        }

        public override void Exit()
        {
            Debug.Log("ChaseState - Exit");
            
            controller.OnChasePlayerCompleted -= HandleOnChasePlayerCompleted;
        }

        private void HandleOnChasePlayerCompleted()
        {
            OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.CHASE, newState = States.IDLE } );
            Exit();
        }
    }
}
