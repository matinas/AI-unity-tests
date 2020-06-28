using UnityEngine;
using AITests.Pathfinding;
using System.Collections.Generic;
using System.Linq;

// Basic FollowWaypoints functionality. For more complex features like tunable snapping rotation,
// rotation smoothness, etc) check FollowWaypoints.cs inside "2. Waypoints" Folder

public class FollowWaypointsPath : MonoBehaviour
{
    private bool _startPathTraversal;
    
    private IEnumerable<WaypointNode> _waypointsPath;

    private int _currentPathNode;

    private float _accuracy = 0.5f;

    [SerializeField]
    private float Speed;

    void Start()
    {
        _startPathTraversal = false;
        _currentPathNode = 0;

        PathfindingManager.Instance.OnAStarPathComputed += HandleAStarPathComputed;
    }

    void LateUpdate()
    {
        if (_startPathTraversal)
        {
            if (_currentPathNode < _waypointsPath.Count())
            {
                WaypointNode node = _waypointsPath.ElementAt(_currentPathNode);

                var targetDir = node.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(targetDir.x, 0.0f, targetDir.z), transform.up);
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);
                
                if (Vector3.Distance(transform.position, node.transform.position) <= _accuracy)
                {
                    _currentPathNode++; // move to the next node in the path
                    Debug.Log($"Moved to the node #{_currentPathNode} in the waypoint path list");
                }
            }
            else // finished path traversal
            {
                _startPathTraversal = false;
                _currentPathNode = 0;
            }
        }
    }

    private void HandleAStarPathComputed(IEnumerable<WaypointNode> waypoints)
    {
        _waypointsPath = waypoints;
        _startPathTraversal = true;
    }
}
