using System;
using UnityEngine;

namespace AITests.FSM.States
{
    public class SleepState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;

        private bool _readyToSleep = false;

        public SleepState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("SleepState - Enter");

            controller.OnWaypointReached += HandleWaypointReached;
            controller.OnNapCompleted += HandleNapCompleted;

            controller.GoToWaypoint(0);
        }

        public override void Update()
        {
            if (_readyToSleep)
            {
                Debug.Log("SleepState - Update");

                controller.TakeANap(10.0f);
            }
        }

        public override void Exit()
        {
            Debug.Log("SleepState - Exit");

            controller.OnWaypointReached -= HandleWaypointReached;
            controller.OnNapCompleted -= HandleNapCompleted;
        }

        public void HandleWaypointReached(int wp)
        {
            if (wp == 0)
            {
                Debug.Log("Ready to take a nap!");
                
                _readyToSleep = true;
            }
        }

        public void HandleNapCompleted()
        {
            OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.SLEEP, newState = States.IDLE } );
            Exit();
        }
    }
}
