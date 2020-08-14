using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AITests.GOAP.Actions;
using System;

namespace AITests.GOAP
{
    public class GOAPAgent : MonoBehaviour
    {
        private List<GOAPAction> _actions;

        private WorldState _localState;

        private GOAPPlan _currentPlan;

        [SerializeField]
        public WorldStatePair[] Goal;

        // Start is called before the first frame update
        void Start()
        {
            _actions = gameObject.GetComponents<GOAPAction>().ToList();

            // disable all action behaviors as each one will be activated individually as needed when executing the plan
            foreach (var action in _actions)
            {
                action.enabled = false;
            }

            _localState = new WorldState();
            _localState.AddState(WorldStateAttribute.ToolEquipped, false);
            _localState.AddState(WorldStateAttribute.HasTool, false);
            _localState.AddState(WorldStateAttribute.StoneCollected, false);
            _localState.AddState(WorldStateAttribute.FishCollected, false);
            _localState.AddState(WorldStateAttribute.WoodCollected, false);
            _localState.AddState(WorldStateAttribute.StoneStored, false);
            _localState.AddState(WorldStateAttribute.FishStored, false);
            _localState.AddState(WorldStateAttribute.WoodStored, false);
            _localState.AddState(WorldStateAttribute.ToolCrafted, false);
        }

        void Update()
        {
            if (_currentPlan == null || _currentPlan.Status == GOAPPlan.PlanStatus.Invalid ||
                                        _currentPlan.Status == GOAPPlan.PlanStatus.Completed ||
                                        _currentPlan.Status == GOAPPlan.PlanStatus.Aborted)
            {
                _currentPlan = GOAPPlanner.Instance.ComputePlan(_actions, Goal[0], _localState, WorldManager.Instance.GlobalState); // get a new plan
                
                // Debug.Log("A new plan has just been computed");

                if (_currentPlan.Status == GOAPPlan.PlanStatus.Valid)
                {
                    _currentPlan.Execute(); // execute the current plan
                }
            }
        }
    }
}