using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using System.Linq;
using System.Collections;

namespace AITests.GOAP.Actions
{
    public abstract class GOAPAction : MonoBehaviour, IGOAPAction
    {
        public event Action OnActionCompleted;
        public event Action OnActionAborted;

        public WorldState Preconditions { get; set; }
        public WorldState Effects { get; set; }

        [SerializeField]
        [Tooltip("If the preconditions are filled here, these are the ones set for the action. Otherwise preconditions are set with pre-defined values")]
        private WorldStatePair[] Preconditions_Inspector;

        [SerializeField]
        [Tooltip("If the effects are filled here, these are the ones set for the action. Otherwise effects are set with pre-defined values")]
        private WorldStatePair[] Effects_Inspector;

        [SerializeField]
        [Tooltip("Does the action requires to be in certain range from a target location to take place?")]
        private bool RangeRequired;

        [SerializeField]
        [Tooltip("Specifies the tarjet location if range is required")]
        private Transform Target;

        [SerializeField]
        [Tooltip("Cost of carrying out the action")]
        private float Cost;

        [SerializeField]
        [Tooltip("Duration of the action, the time the complete action will take to complete")]
        protected float Duration;

        protected bool _completed = false;

        private bool _inRange = false;

        private NavMeshAgent _agent;

        private Coroutine WaitDurationCrt;

        public abstract void SetFixedPreconditions();
        public virtual void SetProceduralPreconditions() {}
        public abstract void SetFixedEffects();

        public virtual bool PreRun() { return true; } // validates whether the conditions are met at action's run time for the action to execute

        public virtual bool PostRun() { return true; } // makes whatever update is needed after the action is completed (world state changes, agent state changes, etc)

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            Preconditions = new WorldState();
            Effects = new WorldState();

            if (Preconditions_Inspector.Length > 0)
            {
                foreach (var prec in Preconditions_Inspector)
                {
                    Preconditions.AddState(prec.Attr, prec.Value);
                }
            }

            if (Effects_Inspector.Length > 0)
            {
                foreach (var effect in Effects_Inspector)
                {
                    Effects.AddState(effect.Attr, effect.Value);
                }
            }

            Init();
            Reset();
        }

        void Update()
        {
            if (!_inRange) // we need to be in certain target range before executing the action, we move there first
            {
                if (!_agent.hasPath)
                {
                    _agent.SetDestination(Target.position);
                }
                else if (_agent.remainingDistance < 0.5f)
                {
                    _inRange = true;

                    if (!PreRun())
                    {
                        Debug.Log($"Action {this.GetType().ToString()} cannot be executed right now and will be aborted");

                        OnActionAborted.Invoke();
                        Reset(); // get the action ready for the next plan execution (as they are re-used on demand)
                    }
                }
            }
            else // we are already in range, we just run the action update loop
            {
                if (Run())
                {
                    if (PostRun()) OnActionCompleted.Invoke();
                    else OnActionAborted.Invoke();

                    Reset(); // get the action ready for the next plan execution (as they are re-used on demand)
                }
            }
        }

        public void Init()
        {
            if (Preconditions_Inspector == null || Preconditions_Inspector.Length == 0) // if there are no preconditions from the inspector, fill them manually
            {
                SetFixedPreconditions();
            }

            SetProceduralPreconditions(); // we always need to manually add procedural precondition
            
            if (Effects_Inspector == null || Effects_Inspector.Length == 0) // if there are no effects from the inspector, fill them manually
            {
                SetFixedEffects();
            }
        }

        public virtual bool Run()
        {
            if (WaitDurationCrt == null)
            {
                WaitDurationCrt = StartCoroutine(WaitDuration());
            }

            if (_completed)
            {
                Debug.Log("Wait completed! " + GetType().ToString());

                StopCoroutine(WaitDurationCrt);
                WaitDurationCrt = null;

                return true;
            }
            
            return false;
        }

        IEnumerator WaitDuration()
        {
            yield return new WaitForSeconds(Duration);

            _completed = true;
        }

        // checks whether the parameter state meets the requirement to execute the action
        public bool AllPreconditionsMet(WorldState state)
        {
            return Preconditions.States.All(x => IsPreconditionMet(x, state));
        }

        // returns all the action's preconditions that are not met for the given world state, or null if all are met
        public List<WorldStateKeyPair> GetNotMetPreconditions(WorldState state)
        {
            List<WorldStateKeyPair> notMet = new List<WorldStateKeyPair>();

            foreach (var prec in Preconditions.States)
            {
                if (!IsPreconditionMet(prec, state))
                {
                    notMet.Add(NormalizePrecondition(prec));
                }
            }

            return notMet;
        }

        private bool IsPreconditionMet(KeyValuePair<WorldStateAttribute, object> prec, WorldState state)
        {
            if (prec.Value.GetType().IsPrimitive) // primitive type, commonly a bool precondition for now
            {
                return state.CheckStateValue(prec.Key, prec.Value);
            }
            else if (prec.Value.GetType() == typeof(Func<bool>)) // procedural precondition
            {
                return ((Func<bool>) prec.Value).Invoke();
            }

            return false;
        }

        // normalizes/evaluates a precondition in the sense that a simple primitive precondition with integer/bool values, will be just kept as it is,
        // but a procedural precondition will be evaluated, and get assigned the opposite value of the check result (commonly true, as the procedural precs
        // are of the form CheckMaterialAvailability, CheckToolAvailable, etc. and will evaluate to false if not met), so an effect that matches can be found later
        private WorldStateKeyPair NormalizePrecondition(KeyValuePair<WorldStateAttribute, object> prec)
        {
            if (prec.Value.GetType().IsPrimitive) // primitive type, commonly a bool precondition for now
            {
                return new WorldStateKeyPair(prec.Key, prec.Value);
            }
            else if (prec.Value.GetType() == typeof(Func<bool>)) // procedural precondition
            {
                var procCheck = ((Func<bool>) prec.Value).Invoke();
                return new WorldStateKeyPair(prec.Key, WorldState.NormalizeValue(!procCheck));
            }

            return new WorldStateKeyPair();
        }

        // filters the actions passed as parameter to return only those that matchs the current action (effects matches any current action's preconditions)
        public IEnumerable<GOAPAction> Match(IEnumerable<GOAPAction> actions)
        {
            return actions.Where(x => MatchAny(x.Effects));
        }

        public bool Match(WorldStateKeyPair ws)
        {
            var b = Effects.CheckStateValue(ws.Attr, ws.Value);
            return b;
        }

        private bool MatchAny(WorldState effects)
        {
            foreach (var prec in Preconditions.States)
            {
                if (effects.CheckStateValue(prec.Key, prec.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            _inRange = !RangeRequired;
            _completed = false;
            _agent.ResetPath();
        }

        public float GetCost() { return Cost; }
    }
}
