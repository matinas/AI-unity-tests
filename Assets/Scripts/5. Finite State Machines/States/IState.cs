using System;
using UnityEngine;

namespace AITests.FSM.States
{
    public enum States
    {
        IDLE,
        PATROL,
        CHASE,
        SLEEP
    }

    public interface IState
    {
        event Action<StateChangedEvt> OnStateChangedEvent;

        void Enter();
        void Update();
        void Exit();
    }
}