using System;
using UnityEngine;

namespace AITests.AMA.States
{
    public abstract class State : IState
    {
        public abstract event Action<StateChangedEvt> OnStateChangedEvent;

        protected BotController controller;

        public State(Transform go)
        {
            controller = go.GetComponent<BotController>();
            Enter();
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
