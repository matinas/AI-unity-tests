using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AITests.Pathfinding
{
    public delegate void AStarPathComputed(List<WaypointNode> nodes);

    public class PathfindingManager : MonoBehaviour
    {
        public event AStarPathComputed AStarPathComputed;

        public Action<IEnumerable<WaypointNode>> OnAStarPathComputed;

        private static PathfindingManager _instance;

        public static PathfindingManager Instance
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
        private Transform _player;

        void Awake()
        {
            _instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            try
            {
                _waypoints = GameObject.FindObjectsOfType<WaypointNode>().OrderBy(w => w.ID);
                AStarPathfinder.Instance.Init(_waypoints);

                Debug.Log("Waypoints graph created successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Couldn't generate the waypoints graph. Message: {e.Message}");
            }

            InputManager.Instance.OnWaypointClicked += HandleWaypointClicked;
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
                WaypointNode from = ClosestToPlayer();
                List<int> path = AStarPathfinder.Instance.FindAStarPath(from.ID, wp.ID);

                Debug.Log($"Path from node {from.ID} to {wp.ID} is: " + string.Join(", ", path.Select(x => x.ToString())));

                // select the proper waypoint nodes based on the calculated path
                IEnumerable<WaypointNode> wpPath = path.ConvertAll<WaypointNode>(x => _waypoints.Where(w => w.ID == x).FirstOrDefault());

                OnAStarPathComputed.Invoke(wpPath);
                //AStarPathComputed.Invoke(wpPath);
            }
        }

        private WaypointNode ClosestToPlayer()
        {
            WaypointNode closest = _waypoints.ElementAt(0);
            float closestDist = float.MaxValue;

            foreach (var wp in _waypoints)
            {
                var playerWPDistance = Vector3.Distance(_player.transform.position, wp.transform.position);
                if (playerWPDistance < closestDist)
                {
                    closestDist = playerWPDistance;
                    closest = wp;
                }
            }

            return closest;
        }
    }
}