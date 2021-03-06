﻿using System.Collections.Generic;
using UnityEngine;

namespace AITests.Pathfinding
{
    // waypoints nodes are set in the scene based on the "Scene graph.png" graph inside "3. Pathfinding" folder

    public class WaypointNode : MonoBehaviour
    {
        public int ID;
        public List<WaypointNode> AdjacentNodes;
        public List<float> EdgeCosts;

        // Start is called before the first frame update
        void Start()
        {
            if (AdjacentNodes.Count != EdgeCosts.Count)
            {
                Debug.LogError("The costs list should have the same amount of items as the adjacent nodes list (one cost per adjacent node)");
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}