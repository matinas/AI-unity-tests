using System;

namespace AITests.AMA.States
{
    public enum States
    {
        SEEK,
        HIDE,
        WANDER
    }

    public interface IState
    {
        event Action<StateChangedEvt> OnStateChangedEvent;

        void Enter();
        void Update();
        void Exit();
    }
}