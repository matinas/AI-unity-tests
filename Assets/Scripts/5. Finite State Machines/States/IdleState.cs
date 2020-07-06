using System;
using UnityEngine;

namespace AITests.FSM.States
{
    // check the "FSM.png" image inside "5. Finite State Machines" folder
    // for the FSM diagram in which the state transitions are based

    public class IdleState : State
    {
        private bool _readyToIdle = false;

        public override event Action<StateChangedEvt> OnStateChangedEvent;

        public IdleState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("IdleState - Enter");

            controller.OnWaypointReached += HandleWaypointReached;
            controller.OnSpinAroundCompleted += HandleSpinAroundCompleted;

            controller.GoToWaypoint(0);
        }

        public override void Update()
        {
            if (_readyToIdle)
            {
                Debug.Log("IdleState - Update");
                controller.SpinAround(10.0f);
            }
        }

        public override void Exit()
        {
            Debug.Log("IdleState - Exit");
            _readyToIdle = false;

            controller.OnWaypointReached -= HandleWaypointReached;
            controller.OnSpinAroundCompleted -= HandleSpinAroundCompleted;
        }

        public void HandleWaypointReached(int wp)
        {
            if (wp == 0)
            {
                Debug.Log("Ready to Idle!");
                _readyToIdle = true;
            }
        }

        public void HandleSpinAroundCompleted()
        {
            OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.IDLE, newState = States.PATROL } );
            Exit();
        }
    }
}
