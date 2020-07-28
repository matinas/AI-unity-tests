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
        protected Transform Target;

        protected NavMeshAgent _agent;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();

            Init();
        }

        void Update()
        {
            if (RangeRequired && !_agent.hasPath)
            {
                _agent.SetDestination(Target.position);
            }
            
            if (_agent.hasPath && _agent.remainingDistance < 0.5f)
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
    }
}
