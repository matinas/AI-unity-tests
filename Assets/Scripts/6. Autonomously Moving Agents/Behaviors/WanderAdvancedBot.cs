using UnityEngine;
using UnityEngine.AI;

public class WanderAdvancedBot : MonoBehaviour
{
    private NavMeshAgent _agent;

    [SerializeField]
    private float Distance;

    [SerializeField]
    private float Radius;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private bool RandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        float y = Random.Range(-1.0f, 1.0f);
        float x = Mathf.Sqrt(radius - Mathf.Pow(y,2)); // formula of the circle X²+Y²=r to get a point in the circle's circumference

        var rand = center + new Vector3(x, 0.0f, y);
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
        if (!_agent.hasPath || _agent.remainingDistance < 1f)
        {
            var center = transform.forward * Mathf.Sign(Random.Range(-0.5f, 1.0f)) * Distance; // mult by the sign allows the agent to choose to go backwards eventually
            
            Vector3 randomPoint;
            if (RandomPoint(center, Radius, out randomPoint))
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
