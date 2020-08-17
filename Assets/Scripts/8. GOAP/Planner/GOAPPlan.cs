using System.Collections.Generic;
using System.Linq;
using AITests.GOAP.Actions;
using UnityEngine;

namespace AITests.GOAP.Planner
{
    public class GOAPPlan
    {
        public enum PlanStatus
        {
            Valid,
            Invalid,
            InProgress,
            Completed,
            Aborted,
        }

        public IEnumerable<GOAPAction> Actions { get; private set; } // we could have implemented this as a queue, and just pop actions as
                                                                     // they get completed (avoids maintaining current action and index)
        public PlanStatus Status { get; private set; }

        private GOAPAction _currentAction;
        private int _currentActionIdx = 0;

        public GOAPPlan(IEnumerable<GOAPAction> actions)
        {
            Actions = actions;
            Status = PlanStatus.Valid;
        }

        public void Execute()
        {
            if (Status == PlanStatus.Valid)
            {
                _currentAction = Actions.ElementAt(_currentActionIdx);
                _currentAction.OnActionCompleted += HandleActionCompleted;
                _currentAction.OnActionAborted += HandleActionAborted;

                DisableAllButCurrent();

                Status = PlanStatus.InProgress;
            }
        }

        private void HandleActionCompleted()
        {
            _currentActionIdx++;
            _currentAction.OnActionCompleted -= HandleActionCompleted; // unsuscribe from previous action completition event
            _currentAction.OnActionAborted -= HandleActionAborted; // unsuscribe from previous action abortion event

            if (_currentActionIdx < Actions.Count())
            {
                _currentAction = Actions.ElementAt(_currentActionIdx);
                _currentAction.OnActionCompleted += HandleActionCompleted; // suscribe to new action completition event
                _currentAction.OnActionAborted += HandleActionAborted; // suscribe to new action abortion event

                DisableAllButCurrent();
            }
            else
            {
                Status = PlanStatus.Completed;
            }
        }

        private void HandleActionAborted()
        {
            _currentAction.OnActionCompleted -= HandleActionCompleted; // unsuscribe from previous action completition event
            _currentAction.OnActionAborted -= HandleActionAborted; // unsuscribe from previous action abortion event

            Status = PlanStatus.Aborted;
        }

        private void DisableAllButCurrent() // disables all the actions in the game object except for the current one (avoids unnecessary action updates)
        {
            _currentAction.enabled = true;

            var otherActions = _currentAction.gameObject.GetComponents<GOAPAction>().Where(x => !GameObject.ReferenceEquals(_currentAction, x));
            foreach (var action in otherActions)
            {
                action.enabled = false;
            }
        }
    }
}