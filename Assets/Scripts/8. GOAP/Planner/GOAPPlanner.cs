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
            var actionPlans = ProcessPlannerGraph(dummyPlannerState);

            // TODO: filter the different plans so to get the less costly of them

            //actionPlans.Reverse();
            return new GOAPPlan(actionPlans.ElementAt(0)); // FIXME: returning the first plan for now, but it should be the less costly
        }

        private List<LinkedList<GOAPAction>> ProcessPlannerGraph(GOAPPlannerState state)
        {
            if (state.Children.Count == 0)
            {
                if (!state.IsFulfilled)
                {
                    return null;
                }
                else
                {
                    var singleActionList = new LinkedList<GOAPAction>();
                    singleActionList.AddFirst(state.CurrentAction);

                    var listOfActions = new List<LinkedList<GOAPAction>>();
                    listOfActions.Add(singleActionList);

                    return listOfActions;
                }
            }
            else
            {
                List<LinkedList<GOAPAction>> actionList = new List<LinkedList<GOAPAction>>();
                foreach (var child in state.Children)
                {
                    var childActionLists = ProcessPlannerGraph(child);

                    if (childActionLists != null) // add the current action to the children paths
                    {
                        foreach (var list in childActionLists)
                        {
                            if (state.CurrentAction != null) list.AddFirst(state.CurrentAction); // avoids adding null for the actionless dummy node
                        }
                        actionList.AddRange(childActionLists);
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