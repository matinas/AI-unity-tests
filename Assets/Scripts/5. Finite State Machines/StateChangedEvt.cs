namespace AITests.FSM.States
{
    public struct StateChangedEvt
    {
        public States oldState;
        public States newState;
    }
}