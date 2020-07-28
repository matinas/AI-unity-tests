using System.Collections;
using System.Collections.Generic;
using AITests.GOAP.Actions;
using UnityEngine;
using System.Linq;

public class GOAPPlan
{
    public enum PlanStatus
    {
        Valid,
        Invalid,
        InProgress,
        Completed,
    }

    public List<GOAPAction> Actions { get; private set; }
    public PlanStatus Status { get; private set; }

    private GOAPAction _currentAction;
    private int _currentActionIdx = 0;

    public GOAPPlan()
    {
        Actions = null;
        Status = PlanStatus.Invalid;
    }

    public GOAPPlan(List<GOAPAction> actions)
    {
        Actions = actions;
        Status = PlanStatus.Valid;

        _currentAction = Actions[_currentActionIdx];
        _currentAction.OnActionCompleted += HandleActionCompleted;

        DisableAllButCurrent();
    }

    public void ExecuteOrUpdate()
    {
        if (Status == PlanStatus.Valid)
        {
            Status = PlanStatus.InProgress;
        }
    }

    private void HandleActionCompleted()
    {
        _currentActionIdx++;
        _currentAction.OnActionCompleted -= HandleActionCompleted; // unsuscribe from previous action completition event

        if (_currentActionIdx < Actions.Count)
        {
            _currentAction = Actions[_currentActionIdx];
            _currentAction.OnActionCompleted += HandleActionCompleted; // suscribe to new action completition event

            DisableAllButCurrent();
        }
        else
        {
            Status = PlanStatus.Completed;
        }
    }

    private void DisableAllButCurrent() // disables all the actions in the game object, except for the current one (this avoids unnecessary action updates)
    {
        _currentAction.enabled = true;

        var otherActions = _currentAction.gameObject.GetComponents<GOAPAction>().Where(x => !GameObject.ReferenceEquals(_currentAction, x));
        foreach (var action in otherActions)
        {
            action.enabled = false;
        }
    }
}
