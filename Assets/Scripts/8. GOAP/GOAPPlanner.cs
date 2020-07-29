using System.Collections.Generic;
using AITests.GOAP.Actions;
using UnityEngine;
using System.Linq;

namespace AITests.GOAP
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

        public GOAPPlan ComputePlan(List<GOAPAction> actions, Dictionary<WorldStateAttribute, object> worldState, GOAPGoal goal)
        {
            // FIXME: just for testing, always returns the first three actions
            var acs = actions.Take(3).ToList();

            return new GOAPPlan(acs);
        }
    }
}