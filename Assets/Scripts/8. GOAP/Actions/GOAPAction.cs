using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

namespace AITests.GOAP.Actions
{
    public abstract class GOAPAction : MonoBehaviour, IGOAPAction
    {
        public event Action OnActionCompleted;

        public Dictionary<WorldStateAttribute, object> Preconditions { get; set; }
        public Dictionary<WorldStateAttribute, object> Effect { get; set; }

        [SerializeField]
        private bool RangeRequired;

        [SerializeField]
        private Transform Target;

        [SerializeField]
        private int Cost;

        private NavMeshAgent _agent;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            Init();
        }

        void Update()
        {
            if (RangeRequired && !_agent.hasPath) // if we need to be in certain target range before executing the action, we move there first
            {
                _agent.SetDestination(Target.position);
            }
            else if (!RangeRequired)
            {
                if (Run())
                {
                    OnActionCompleted.Invoke();
                }
            }

            if (_agent.hasPath && _agent.remainingDistance < 0.5f) // if we are already in range, we just run the action update loop
            {
                if (Run())
                {
                    OnActionCompleted.Invoke();
                    _agent.ResetPath();
                }
            }
        }

        public abstract void Init();
        public abstract bool Run();

        protected void AddPrecondition(WorldStateAttribute attr, object value)
        {
            if (Preconditions == null)
            {
                Preconditions = new Dictionary<WorldStateAttribute, object>();
            }

            Preconditions.Add(attr, value);
        }

        protected void AddEffect(WorldStateAttribute attr, object value)
        {
            if (Effect == null)
            {
                Effect = new Dictionary<WorldStateAttribute, object>();
            }

            Effect.Add(attr, value);
        }
    }
}
