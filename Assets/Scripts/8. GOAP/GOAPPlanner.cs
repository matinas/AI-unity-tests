using System.Collections.Generic;
using AITests.GOAP.Actions;
using System.Linq;
using System;
using UnityEngine;
using System.Collections;

namespace AITests.GOAP
{
    public class GOAPPlanner
    {
        public class PlannerState
        {
            public WorldState CurrentState { get; set; }
            public WorldState DesiredState { get; set; }

            public List<GOAPAction> AvailableActions { get; private set; }

            public PlannerState(List<GOAPAction> actions, WorldState state)
            {
                CurrentState = new WorldState(state);
                DesiredState = new WorldState();

                AvailableActions = actions.ToList();
            }

            public void AddDesiredState(WorldStateAttribute attr, object desiredValue)
            {
                DesiredState.AddOrUpdateState(attr, WorldState.NormalizeValue(Convert.ToInt32(desiredValue)));
            }

            public void UpdateCurrentState(WorldStateAttribute attr, object newValue)
            {
                CurrentState.UpdateState(attr, newValue);
            }

            public WorldStateKeyPair GetFirstNotFulfilled()
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
        }

        private static GOAPPlanner _instance;

        public static GOAPPlanner Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GOAPPlanner();

                return _instance;
            }
        }

        public GOAPPlan ComputePlan(List<GOAPAction> actions, WorldStateKeyPair goal, WorldState localState, WorldState worldState)
        {
            List<GOAPAction> planActions = new List<GOAPAction>();
            Queue<PlannerState> statesStack = new Queue<PlannerState>();

            WorldState globalState = localState + worldState;

            PlannerState plannerState = new PlannerState(actions, globalState);
            plannerState.AddDesiredState(goal.Attr, goal.Value);
            
            statesStack.Enqueue(plannerState); // queue the current planner state

            while (statesStack.Count > 0)
            {
                var currPlannerState = statesStack.Dequeue();
                var availableActions = currPlannerState.AvailableActions;

                Debug.Log($"Stack now has {statesStack.Count} planner states");

                while (!currPlannerState.AllStatesFulfilled())
                {
                    var toFulfill = plannerState.GetFirstNotFulfilled();
                    var matchingActions = availableActions.Where(x => x.Match(toFulfill)).ToList();

                    foreach (var action in matchingActions)
                    {
                        var notMetPrecs = action.GetNotMetPreconditions(currPlannerState.CurrentState);
                        foreach (var prec in notMetPrecs)
                        {
                            plannerState.AddDesiredState(prec.Attr, prec.Value);
                        }

                        plannerState.UpdateCurrentState(toFulfill.Attr, toFulfill.Value);
                        planActions.Add(action);
                        availableActions.Remove(action);
                    }
                }
            }

            planActions.Reverse();
            return new GOAPPlan(planActions);
        }
    }
}