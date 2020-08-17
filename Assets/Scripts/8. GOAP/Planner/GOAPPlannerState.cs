using System.Collections.Generic;
using System.Linq;
using System;
using AITests.GOAP.Actions;

namespace AITests.GOAP.Planner
{
    public class GOAPPlannerState
    {
        public WorldState CurrentState { get; set; }
        public WorldState DesiredState { get; set; }

        public List<GOAPAction> AvailableActions { get; private set; }

        public GOAPAction CurrentAction { get; private set; }

        public float CurrentCost { get; private set; }

        public GOAPPlannerState Parent { get; set; }

        public List<GOAPPlannerState> Children { get; private set; }

        public bool IsFulfilled { get; private set; }

        public GOAPPlannerState(List<GOAPAction> availableActions, WorldState state)
        {
            CurrentState = new WorldState(state);
            DesiredState = new WorldState();

            AvailableActions = availableActions.ToList();
            Children = new List<GOAPPlannerState>();

            IsFulfilled = false;
            Parent = null;
        }

        public void AddDesiredState(WorldStateAttribute attr, object desiredValue)
        {
            DesiredState.AddOrUpdateState(attr, WorldState.NormalizeValue(Convert.ToInt32(desiredValue)));
        }

        public void UpdateCurrentState(WorldStateAttribute attr, object newValue)
        {
            CurrentState.UpdateState(attr, newValue);

            if (AllStatesFulfilled()) IsFulfilled = true;
        }

        public WorldStateKeyPair GetFirstNotFulfilled() // returns the first not fulfilled state, or null if they are all fulfilled
        {
            foreach (var state in DesiredState.States)
            {
                if (!IsStateFulfilled(state.Key))
                {
                    return new WorldStateKeyPair(state.Key, Convert.ToInt32(DesiredState.States[state.Key]));
                }
            }

            return new WorldStateKeyPair();
        }

        public bool AllStatesFulfilled()
        {
            return DesiredState.States.All(x => IsStateFulfilled(x.Key));
        }

        public bool IsStateFulfilled(WorldStateAttribute attr)
        {
            if ((CurrentState.GetStateValue(attr) == null) || (DesiredState.GetStateValue(attr) == null)) return false;

            return CurrentState.CheckStateValue(attr, DesiredState.GetStateValue(attr));
        }

        public void AddPlanAction(GOAPAction action)
        {
            CurrentAction = action;
            CurrentCost += action.GetCost();
        }

        public void MarkFulfilled(bool fulfilled)
        {
            IsFulfilled = fulfilled;
        }

        public void AddChildrenState(GOAPPlannerState state)
        {
            Children.Add(state);
        }
    }
}