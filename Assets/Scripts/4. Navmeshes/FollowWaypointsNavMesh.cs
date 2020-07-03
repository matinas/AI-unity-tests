using UnityEngine;
using UnityEngine.AI;

public class FollowWaypointsNavMesh : MonoBehaviour
{
    private NavMeshAgent navmeshAgent;

    void Start()
    {
         navmeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        navmeshAgent.SetDestination(destination);
    }
}
