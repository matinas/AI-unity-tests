using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AITests.GOAP.Actions;

namespace AITests.GOAP.Planner
{
    public class GOAPPlanner
    {
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

        // Follows a simple Depth First Search approach, not A* (no heuristics taken into account to select paths)
        public GOAPPlan ComputePlan(List<GOAPAction> actions, WorldStateKeyPair goal, WorldState localState, WorldState worldState)
        {
            Stack<GOAPPlannerState> statesStack = new Stack<GOAPPlannerState>();
            WorldState globalState = localState + worldState;

            GOAPPlannerState dummyPlannerState = new GOAPPlannerState(actions, globalState); // this state is added just to maintain the complete set of available actions and
            dummyPlannerState.AddDesiredState(goal.Attr, goal.Value);                        // world state, but it won't have any plan's actions (kinda used to just represent the goal)
            
            statesStack.Push(dummyPlannerState); // add the initial planner state to the stack (NOTE: we could have done it recursively instead of maintaining a states stack)

            GOAPPlannerState currPlannerState;
            while (statesStack.Count > 0)
            {
                currPlannerState = statesStack.Pop();
                // Debug.Log($"Popped state from the stack. Stack now has {statesStack.Count} planner states");

                var availableActions = currPlannerState.AvailableActions;

                bool endPlanProcessing = false;
                while (!currPlannerState.IsFulfilled && !endPlanProcessing) // while we still have states to fulfill
                {
                    var toFulfill = currPlannerState.GetFirstNotFulfilled();
                    var matchingActions = availableActions.Where(x => x.Match(toFulfill)).ToList();

                    if (matchingActions.Count > 0)
                    {
                        foreach (var action in matchingActions)
                        {
                            var newActionState = GenerateNewActionState(action, availableActions.Where(x => !matchingActions.Contains(x)).ToList(), currPlannerState);
                            currPlannerState.AddChildrenState(newActionState);

                            statesStack.Push(newActionState);
                            // Debug.Log($"Pushed state to the stack. Stack now has {statesStack.Count} planner states");
                        }

                        currPlannerState.UpdateCurrentState(toFulfill.Attr, toFulfill.Value);
                        endPlanProcessing = currPlannerState.IsFulfilled;
                    }
                    else // we reached a point in which the plan isn't fulfilled and it has no more actions to take, so we stop processing it
                    {
                        endPlanProcessing = true;
                    }
                }
            }

            // process the generated graph so to get the fulfilled action paths
            var actionPaths = ProcessPlannerGraph(dummyPlannerState);

            // find the least costly path from the list of available-to-choose paths
            var bestActionPath = actionPaths.OrderBy(x => x.Cost).First();

            return new GOAPPlan(bestActionPath.Actions);
        }

        private IEnumerable<GOAPPlannerPath> ProcessPlannerGraph(GOAPPlannerState state)
        {
            if (state.Children.Count == 0)
            {
                if (!state.IsFulfilled)
                {
                    return null;
                }
                else
                {
                    var singleActionPath = new GOAPPlannerPath();
                    singleActionPath.AddAction(state.CurrentAction); // this also registers the cost for the action

                    var listOfPaths = new List<GOAPPlannerPath>();
                    listOfPaths.Add(singleActionPath);

                    return listOfPaths;
                }
            }
            else
            {
                List<GOAPPlannerPath> actionList = new List<GOAPPlannerPath>();
                foreach (var child in state.Children)
                {
                    var childActionPaths = ProcessPlannerGraph(child);

                    if (childActionPaths != null) // add the current action to the children paths
                    {
                        foreach (var path in childActionPaths)
                        {
                            if (state.CurrentAction != null) path.AddAction(state.CurrentAction); // avoids adding null for the actionless dummy node
                        }
                        actionList.AddRange(childActionPaths);
                    }
                }

                return actionList;
            }
        }

        private GOAPPlannerState GenerateNewActionState(GOAPAction action, List<GOAPAction> currAvailableAcs, GOAPPlannerState currState)
        {
            GOAPPlannerState newActionState = new GOAPPlannerState(currAvailableAcs, currState.CurrentState);
            newActionState.Parent = currState;
            newActionState.AddPlanAction(action);

            var notMetPrecs = action.GetNotMetPreconditions(newActionState.CurrentState);
            foreach (var prec in notMetPrecs)
            {
                newActionState.AddDesiredState(prec.Attr, prec.Value);
            }

            newActionState.MarkFulfilled(notMetPrecs.Count == 0); // if there aren't any preconditions not met, we mark the state as already fulfilled (will be a branch in the states graph)

            return newActionState;
        }
    }
}