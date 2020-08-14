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

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            if (Preconditions_Inspector.Length > 0)
            {
                foreach (var prec in Preconditions_Inspector)
                {
                    AddPrecondition(prec.Attr, prec.Value);
                }
            }

            if (Effects_Inspector.Length > 0)
            {
                foreach (var effect in Effects_Inspector)
                {
                    AddEffect(effect.Attr, effect.Value);
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
                        Debug.Log("Action cannot be executed right now and will be aborted");

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

        public abstract void Init();

        public virtual void Reset()
        {
            _inRange = !RangeRequired;
            _completed = false;
            _agent.ResetPath();
        }

        public virtual bool PreRun() { return true; } // validates whether the conditions are met at action's run time for the action to execute

        public virtual bool PostRun() { return true; } // makes whatever update is needed after the action is completed (world state changes, agent state changes, etc)

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
        public bool AreRequirementsMet(WorldState state)
        {
            return Preconditions.States.All(x => MeetRequirements(x, state));
        }

        // returns all the action's preconditions that are not met for the given world state, or null if all are met
        public Dictionary<WorldStateAttribute, object> GetNotMetRequirements(WorldState state)
        {
            Dictionary<WorldStateAttribute, object> notMet = new Dictionary<WorldStateAttribute, object>();

            foreach (var prec in Preconditions.States)
            {
                if (!MeetRequirements(prec, state))
                {
                    if (notMet == null) notMet = new Dictionary<WorldStateAttribute, object>();
                    notMet.Add(prec.Key, prec.Value);
                }
            }

            return notMet;
        }

        private bool MeetRequirements(KeyValuePair<WorldStateAttribute, object> prec, WorldState state)
        {
            if (prec.Value.GetType().IsPrimitive) // primitive type, commonly a bool precondition for now
            {
                return state.CheckState(prec.Key, prec.Value);
            }
            else if (prec.Value.GetType() == typeof(Func<bool>)) // procedural precondition
            {
                return ((Func<bool>) prec.Value).Invoke();
            }

            return false;
        }

        // filters the actions passed as parameter to return only those that matchs the current action (effects matches any current action's preconditions)
        public IEnumerable<GOAPAction> Match(IEnumerable<GOAPAction> actions)
        {
            return actions.Where(x => MatchAny(x.Effects));
        }

        public bool Match(KeyValuePair<WorldStateAttribute, int> ws)
        {
            var b = Effects.CheckState(ws.Key, ws.Value);
            return b;
        }

        private bool MatchAny(WorldState effects)
        {
            foreach (var prec in Preconditions.States)
            {
                if (effects.CheckState(prec.Key, prec.Value))
                {
                    return true;
                }
            }

            return false;
        }

        protected void AddPrecondition(WorldStateAttribute attr, object value)
        {
            if (Preconditions == null)
            {
                Preconditions = new WorldState();
            }

            Preconditions.AddState(attr, value);
        }

        protected void AddEffect(WorldStateAttribute attr, object value)
        {
            if (Effects == null)
            {
                Effects = new WorldState();
            }

            Effects.AddState(attr, value);
        }
    }
}
