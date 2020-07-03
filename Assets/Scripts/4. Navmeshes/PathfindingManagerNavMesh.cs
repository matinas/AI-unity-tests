using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AITests.Pathfinding
{
    public class PathfindingManagerNavMesh : MonoBehaviour
    {
        private static PathfindingManagerNavMesh _instance;

        public static PathfindingManagerNavMesh Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("There's no PathfindingManager in the scene to return. Please add one and try again");
                }

                return _instance;
            }
        }

        public IEnumerable<WaypointNode> _waypoints; // these waypoints are set in the scene based on the "Scene graph.png" graph inside "3. Pathfinding" folder

        [SerializeField]
        private IEnumerable<Transform> _agents;

        void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _waypoints = GameObject.FindObjectsOfType<WaypointNode>().OrderBy(w => w.ID);
            _agents = GameObject.FindObjectsOfType<FollowWaypointsNavMesh>().Select(a => a.transform);

            InputManagerNavMesh.Instance.OnWaypointClicked += HandleWaypointClicked;
        }

        public void SetWPColliders(bool enabled)
        {
            foreach (var wp in _waypoints)
            {
                wp.GetComponent<Collider>().enabled = enabled;
            }
        }

        private void HandleWaypointClicked(int id)
        {
            var wp = _waypoints.Where(w => w.ID == id).FirstOrDefault();
            if (wp != null)
            {
                foreach (var agent in _agents)
                {
                    agent.GetComponent<FollowWaypointsNavMesh>()?.SetDestination(wp.transform.position);
                }
            }
        }
    }
}