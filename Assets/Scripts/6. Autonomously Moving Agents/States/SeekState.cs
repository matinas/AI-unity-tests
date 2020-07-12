using System;
using UnityEngine;

namespace AITests.AMA.States
{
    public class SeekState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;
        
        public SeekState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("Enter SeekState");
        }

        public override void Update()
        {
            if (controller.Seek() < 1.5f) // if we are close enough to the player we switch to the hide state
            {
                OnStateChangedEvent.Invoke(new StateChangedEvt { oldState = States.SEEK, newState = States.HIDE });
                Exit();
            }
        }

        public override void Exit()
        {
            Debug.Log("Exit SeekState");
        }
    }
}
