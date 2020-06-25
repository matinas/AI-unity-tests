using UnityEngine;
using System.Linq;
using System;

namespace AITests.Pathfinding
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager _instance;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError("There's no InputManager in the scene to return. Please add one and try again");
                }

                return _instance;
            }
        }

        public Action<int> OnWaypointClicked;

        void Awake()
        {
            _instance = this;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // enable waypoint colliders so we can check if we clicked on a waypoint. kinda hacky but works
                PathfindingManager.Instance.SetWPColliders(true);
                
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                {
                    var go = hitInfo.transform.gameObject;
                    if (go.GetComponent<WaypointNode>() != null)
                    {
                        var waypoint = go.GetComponent<WaypointNode>();
                        OnWaypointClicked.Invoke(waypoint.ID);
                    }
                }

                // disable waypoint colliders. kinda hacky but works
                PathfindingManager.Instance.SetWPColliders(false);
            }
        }
    }
}
