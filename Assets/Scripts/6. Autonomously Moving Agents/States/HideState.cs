using System;
using UnityEngine;

namespace AITests.AMA.States
{
    public class HideState : State
    {
        public override event Action<StateChangedEvt> OnStateChangedEvent;
        
        public HideState(Transform go) : base(go) { }

        public override void Enter()
        {
            Debug.Log("Enter HideState");

    	    controller.OnHiddenFromPlayer += HandleHiddenFromPlayer;
        }

        public override void Update()
        {
            controller.Hide();
        }

        public override void Exit()
        {
            Debug.Log("Exit HideState");

            controller.OnHiddenFromPlayer -= HandleHiddenFromPlayer;
            controller.OnKeepHiddenCompleted -= HandleHiddenTimeout;
        }

        private void HandleHiddenFromPlayer()
        {
            controller.KeepHiddenFor(10.0f);
            controller.OnKeepHiddenCompleted += HandleHiddenTimeout;
        }

        private void HandleHiddenTimeout()
        {
            OnStateChangedEvent?.Invoke(new StateChangedEvt { oldState = States.HIDE, newState = States.WANDER });
            Exit();
        }
    }
}
