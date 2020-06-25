using System;
using System.Collections.Generic;
using AITests.Pathfinding.Graphs;
using AITests.Pathfinding.Graphs.AStar;

namespace AITests.Pathfinding
{
    public class AStarPathfinder
    {
        public static AStarPathfinder Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new AStarPathfinder();
                }

                return _instance;
            }
        }

        private static AStarPathfinder _instance;
        private Graph<AStarNode> _pathGraph;

        public void Init(IEnumerable<WaypointNode> waypoints)
        {
            try
            {
                GenerateWaypointsGraph(waypoints, out _pathGraph);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void GenerateWaypointsGraph(IEnumerable<WaypointNode> waypoints, out Graph<AStarNode> graph)
        {
            graph = new Graph<AStarNode>();

            // add all the nodes
            foreach (var wp in waypoints)
            {
                // add node
                Node<AStarNode> node = new Node<AStarNode>(wp.ID, new AStarNode(wp.transform.position));
                graph.AddNode(node);
            }

            // once all the nodes are added add all the edges
            foreach (var wp in waypoints)
            {
                List<Tuple<Node<AStarNode>, float>> edges = new List<Tuple<Node<AStarNode>, float>>();

                // add adjacent nodes edges
                for (int i=0; i < wp.AdjacentNodes.Count; ++i)
                {
                    Node<AStarNode> adjNode = new Node<AStarNode>(wp.AdjacentNodes[i].ID, new AStarNode(wp.AdjacentNodes[i].transform.position));
                    
                    if (wp.EdgeCosts.Count <= i) throw new Exception($"Can't add edge because there's a cost missing for node with ID {wp.ID}");
                    float cost = wp.EdgeCosts[i];

                    Tuple<Node<AStarNode>, float> edge = new Tuple<Node<AStarNode>, float>(adjNode, cost);
                    edges.Add(edge);
                }
                
                Node<AStarNode> node = graph.GetNode(wp.ID);
                if (node != null)
                {
                    graph.AddEdges(node, edges);
                }
                else
                {
                    throw new Exception($"Can't add edge because the graph doesn't have a node with ID {wp.ID}");
                }
            }
        }

        public List<int> FindAStarPath(int from, int to)
        {
            // TODO

            return null;
        }
    }
}
