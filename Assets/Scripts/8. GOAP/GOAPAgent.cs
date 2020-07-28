using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AITests.GOAP.Actions;
using System;

namespace AITests.GOAP
{
    [Serializable]
    public struct GOAPGoal
    {
        WorldStateAttribute WorldAttr;
        object Value;
    }

    public class GOAPAgent : MonoBehaviour
    {
        private List<GOAPAction> _actions;

        private Dictionary<WorldStateAttribute, object> _worldState;

        private GOAPPlan _currentPlan;

        [SerializeField]
        private GOAPGoal Goal;

        // Start is called before the first frame update
        void Start()
        {
            _actions = gameObject.GetComponents<GOAPAction>().ToList();

            CreateWorldStateForAgent(out _worldState);
            WorldManager.Instance.FillWorldState(ref _worldState);
        }

        private void CreateWorldStateForAgent(out Dictionary<WorldStateAttribute, object> worldState)
        {
            worldState = new Dictionary<WorldStateAttribute, object>();

            foreach (var action in _actions)
            {
                // TODO: add the attributes from all the actions to the local world state maintained by this agent
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentPlan == null || _currentPlan.Status == GOAPPlan.PlanStatus.Invalid || _currentPlan.Status == GOAPPlan.PlanStatus.Completed)
            {
                _currentPlan = GOAPPlanner.Instance.ComputePlan(_actions, _worldState); // get a new plan
            }

            _currentPlan.ExecuteOrUpdate(); // execute the plan
        }
    }
}