using System;
using UnityEngine;

namespace AITests.FSM.States
{
    public abstract class State : IState
    {
        public abstract event Action<StateChangedEvt> OnStateChangedEvent;

        protected EnemyController controller;

        public State(Transform go)
        {
            controller = go.GetComponent<EnemyController>();
            Enter();
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
