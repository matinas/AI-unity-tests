using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AITests.Crowds
{
    public class BotController : MonoBehaviour
    {
        [SerializeField]
        private Transform GoalIni;

        [SerializeField]
        private Transform GoalEnd;

        [SerializeField]
        private Transform Player;

        [SerializeField]
        private float RangeAngle;

        [SerializeField]
        private float RangeDistance;

        [SerializeField]
        private float MaxFleeAngle;

        [SerializeField]
        private float FleeDuration;

        private NavMeshAgent _agent;
        private float initSpeed;
        private bool isFleeing;

        // Start is called before the first frame update
        void Start()
        {
            _agent.SetDestination(GoalIni.transform.position);
            _agent = GetComponent<NavMeshAgent>();

            initSpeed = _agent.speed;
            isFleeing = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isFleeing)
            {
                if (Vector3.Distance(transform.position, GoalIni.transform.position) < 1.0f)
                {
                    _agent.SetDestination(GoalEnd.transform.position);
                }
                else if (Vector3.Distance(transform.position, GoalEnd.transform.position) < 1.0f)
                {
                    _agent.SetDestination(GoalIni.transform.position);
                }

                if (CheckDanger()) // if danger detected, broadcast the FleeAway message to every bot
                {
                    var botsGO = gameObject.transform.parent;
                    botsGO.BroadcastMessage("FleeAway");
                }
            }
        }

        private bool CheckDanger()
        {
            var dirToPlayer = Player.transform.position - transform.position;

            bool playerInVisualRange = Vector3.Angle(transform.forward, dirToPlayer) < RangeAngle;                      // is player within visible range?
            bool playerCloseEnough = Vector3.Distance(transform.position, Player.transform.position) < RangeDistance;   // is player close enough?

            return playerInVisualRange && playerCloseEnough;
        }

        private void FleeAway()
        {
            // to find the flee direction we need to calculate a vector that points in the opposite direction to the direction of movement
            // and the angle between it and the opposite direction is a random angle within a specific range (either left or right)
            Quaternion fleeRot = Quaternion.Euler(0.0f, Mathf.Sign(Random.Range(-1,1)) * Random.Range(0.0f, MaxFleeAngle), 0.0f);
            Vector3 backwardDir = -_agent.transform.forward;
            Vector3 fleeDirection = fleeRot * backwardDir; // apply the rotation

            StartCoroutine(FleeCrt(fleeDirection.normalized));
        }

        IEnumerator FleeCrt(Vector3 dir) // moves the agent in the 'dir' direction for 'FleeDuration' seconds
        {
            isFleeing = true;
            _agent.speed *= 3;

            var time = FleeDuration;
            while (time > 0)
            {
                time -= Time.deltaTime;
                _agent.SetDestination(transform.position + dir);

                yield return null;
            }

            _agent.SetDestination(GoalIni.transform.position);
            _agent.speed = initSpeed;
            
            isFleeing = false;
        }
    }
}
