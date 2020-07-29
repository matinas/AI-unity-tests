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
        private GOAPGoal _goal;

        // Start is called before the first frame update
        void Start()
        {
            _actions = gameObject.GetComponents<GOAPAction>().ToList();

            // disable all action behaviors as each one will be activated individually as needed when executing the plan
            foreach (var action in _actions)
            {
                action.enabled = false;
            }

            _worldState = WorldManager.Instance.GetWorldStateClone(); // fill the world with the complete state
                                                                      // the planner then will use just the relevant states for the set of actions of this agent
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentPlan == null || _currentPlan.Status == GOAPPlan.PlanStatus.Invalid || _currentPlan.Status == GOAPPlan.PlanStatus.Completed)
            {
                _currentPlan = GOAPPlanner.Instance.ComputePlan(_actions, _worldState, _goal); // get a new plan
                Debug.Log("A new plan had just been computed");

                if (_currentPlan.Status == GOAPPlan.PlanStatus.Valid)
                {
                    _currentPlan.Execute(); // execute the current plan
                }
            }
        }
    }
}