using UnityEngine;
using UnityEngine.AI;

public class WanderBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float Range;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 point)
    {
        var randCircle = Random.insideUnitCircle * range;
        var rand = new Vector3(randCircle.x, 0.0f, randCircle.y);
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(rand, out hit, 1.0f, NavMesh.GetAreaFromName("Wakable"))) // check if the position belongs to the NavMesh
        {
            point = hit.position;
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    private void Wander()
    {
        Vector3 randomPoint;
        if (!_agent.hasPath || _agent.remainingDistance < 1f)
        {
            if (RandomPoint(transform.position, Range, out randomPoint))
            {
                _agent.SetDestination(randomPoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }
}
