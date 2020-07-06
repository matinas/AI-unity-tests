using System;
using UnityEngine;

namespace AITests.FSM.States
{
    public class PatrolState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;

        private const int LAPS_TO_PATROL = 1;

        private int _currentIndex; // waypoint index

        private bool _readyToMoveOn;

        private int _laps; // waypoint index

        public PatrolState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("PatrolState - Enter");

            _currentIndex = 0;
            _laps = 0;
            _readyToMoveOn = false;

            controller.OnWaypointReached += HandleOnWaypointReached;
            controller.GoToWaypoint(_currentIndex);
        }

        public override void Update()
        {
            if (_readyToMoveOn)
            {
                Debug.Log("PatrolState - Update");

                if (_currentIndex == 1) // we've just reached wp #0, and we're about to move to wp #1
                {
                    if (_laps == LAPS_TO_PATROL) // laps completed, move to the sleep state
                    {
                        OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.PATROL, newState = States.SLEEP } );
                        Exit();

                        return;
                    }
                    else
                    {
                        _laps++;
                    }
                }

                controller.GoToWaypoint(_currentIndex);
                _readyToMoveOn = false;
            }

            if (controller.DistanceToPlayer() < 2.0f)
            {
                OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.PATROL, newState = States.CHASE } );
                Exit();

                return;
            }
        }

        public override void Exit()
        {
            Debug.Log("PatrolState - Exit");
            
            controller.OnWaypointReached -= HandleOnWaypointReached;
            _readyToMoveOn = false;
        }

        private void HandleOnWaypointReached(int wp)
        {
            if (wp == _currentIndex)
            {
                _currentIndex = (_currentIndex + 1) % controller.WaypointCout();
                _readyToMoveOn = true;
            }
        }
    }
}