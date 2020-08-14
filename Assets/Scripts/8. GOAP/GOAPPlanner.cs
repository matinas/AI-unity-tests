using System.Collections.Generic;
using AITests.GOAP.Actions;
using System.Linq;
using System;
using UnityEngine;

namespace AITests.GOAP
{
    public class GOAPPlanner
    {
        public class PlannerState
        {
            private WorldState currentState;
            private WorldState desiredState;

            public PlannerState()
            {
                currentState = new WorldState();
                desiredState = new WorldState();
            }

            public void AddPlanState(WorldStateAttribute attr, object currentValue, object desiredValue)
            {
                currentState.AddState(attr, currentValue);
                desiredState.AddState(attr, desiredValue);
            }

            public void UpdateValue(WorldStateAttribute attr, object newValue)
            {
                currentState.UpdateState(attr, newValue);
            }

            public bool IsStateFulfilled(WorldStateAttribute attr)
            {
                if ((currentState.GetStateValue(attr) == null) || (desiredState.GetStateValue(attr) == null)) return false;

                return currentState.CheckState(attr, desiredState.GetStateValue(attr));
            }

            public bool AllStatesFulfilled()
            {
                return currentState.States.All(x => IsStateFulfilled(x.Key));
            }

            public KeyValuePair<WorldStateAttribute, int> GetFirstNotFulfilled()
            {
                foreach (var current in currentState.States)
                {
                    if (!IsStateFulfilled(current.Key))
                    {
                        return new KeyValuePair<WorldStateAttribute, int>(current.Key, Convert.ToInt32(desiredState.States[current.Key]));
                    }
                }

                return new KeyValuePair<WorldStateAttribute, int>();
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

        public GOAPPlan ComputePlan(List<GOAPAction> actions, WorldStatePair goal, WorldState localState, WorldState worldState)
        {
            WorldState globalState = localState + worldState;
            PlannerState plannerState = new PlannerState();
            plannerState.AddPlanState(goal.Attr, Convert.ToInt32(globalState.GetStateValue(goal.Attr)), Convert.ToInt32(goal.Value));

            List<GOAPAction> actionList = new List<GOAPAction>();
            bool end = false;
            while (!plannerState.AllStatesFulfilled() && !end)
            {
                var toFulfill = plannerState.GetFirstNotFulfilled();
                var matchingActions = actions.Where(x => x.Match(toFulfill));

                foreach (var action in matchingActions)
                {
                    plannerState.UpdateValue(toFulfill.Key, toFulfill.Value);
                    actionList.Add(action);

                    var notMetReqs = action.GetNotMetRequirements(localState);
                    foreach (var notMetReq in notMetReqs)
                    {
                        plannerState.AddPlanState(notMetReq.Key, localState.GetStateValue(notMetReq.Key), notMetReq.Value);
                    }
                }
            }

            actionList.Reverse();
            return new GOAPPlan(actionList);

            // var acs = actions.Take(4).ToList(); // just for testing purposes, always returns the first four actions
            // return new GOAPPlan(acs);
        }
    }
}