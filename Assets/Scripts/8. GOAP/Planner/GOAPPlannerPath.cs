using System.Collections.Generic;
using AITests.GOAP.Actions;
using System.Linq;

namespace AITests.GOAP.Planner
{
    public class GOAPPlannerPath
    {
        public List<GOAPAction> Actions { get; private set; }

        public float Cost { get; private set; }

        public GOAPPlannerPath()
        {
            Actions = new List<GOAPAction>();
            Cost = 0.0f;
        }

        public void AddAction(GOAPAction action)
        {
            Actions.Add(action);
            Cost += action.GetCost();
        }
    }
}