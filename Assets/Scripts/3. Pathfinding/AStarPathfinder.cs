using System;
using System.Collections.Generic;
using AITests.Pathfinding.Graphs;
using AITests.Pathfinding.Graphs.AStar;
using System.Linq;

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
                    _instance = new AStarPathfinder();
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
                    Node<AStarNode> adjNode = graph.GetNode(wp.AdjacentNodes[i].ID);

                    if (adjNode == null) throw new Exception($"Can't add edge because there's no adjacent node with ID {wp.AdjacentNodes[i].ID}");
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
            List<int> path = new List<int>();

            Node<AStarNode> originNode = _pathGraph.GetNode(from);
            Node<AStarNode> destinationNode = _pathGraph.GetNode(to);

            // fill A* data for the origin node
            originNode.Info.Heuristic = originNode.Info.Cost = 0;
            originNode.Info.Parent = null;

            Node<AStarNode> currentNode = originNode;
            
            List<Node<AStarNode>> openList = new List<Node<AStarNode>>() { currentNode };
            List<Node<AStarNode>> closedList = new List<Node<AStarNode>>() { currentNode };

            // NOTE1: i think we are following more like an actual Dijkstra algorithm here. for a pure A* we should stop
            // searching as soon as we get to the destination node (if the heuristics are well chosen that one should be the best path)

            // NOTE2: we also should better get rid of the cost at the edges entirely and just use the distance between neighbors instead to calculate the G values

            while ((openList.Count != 0) || (currentNode != null))
            {
                openList.Remove(currentNode);

                IEnumerable<Tuple<Node<AStarNode>, float>> adjNodesData = _pathGraph.GetAdjacentNodes(currentNode);
                foreach (var adjNodeData in adjNodesData)
                {
                    ComputeAStarNodeData(adjNodeData, in currentNode, destinationNode.Info.position);
                }

                // TODO: check whether the destination is among the adjacent nodes. If so, we can get the current best path
                // cost to destination, and use that value as a baseline to prune future paths, so if we are trying to build
                // a path which cost is already higher than the best one found we can just prune it (assuming all costs are positive)

                // get the not-yet processed adjacent nodes (not in the Closed list)
                IEnumerable<Node<AStarNode>> adjNodes = adjNodesData.Select(n => n.Item1).Where(x => !closedList.Contains(x));

                if (adjNodes.Count() > 0)
                {
                    // get all the nodes not already contained in the Open list (avoid dups) and add them to the Open list
                    IEnumerable<Node<AStarNode>> newAdjNodes = adjNodes.Where(n => !openList.Contains(n));
                    openList.AddRange(newAdjNodes);

                    // select the best candidate to move into from the adjacent nodes
                    currentNode = SelectBestNeighbour(adjNodes);

                    if (currentNode != null)
                    {
                        openList.Remove(currentNode);
                        closedList.Add(currentNode);
                    }
                }
                else // get any other pending-to-process node in the Open list...
                {
                    currentNode = openList.FirstOrDefault(); // will return null if there aren't nodes pending
                    openList.Remove(currentNode);
                }
            }

            // once the graph was processed and all the A* information filled, we compute the path going backwards from the destination node
            path = ComputeBackwardsPath(destinationNode, originNode);
            path.Reverse();

            ClearAStarData(); // clear graph data so to get it ready for next execution

            return path;
        }

        private void ClearAStarData()
        {
            foreach(var node in _pathGraph.GetNodes())
            {
                node.Info.Reset();
            }
        }

        private void ComputeAStarNodeData(Tuple<Node<AStarNode>, float> nData, in Node<AStarNode> currNode, UnityEngine.Vector3 destPosition)
        {
            Node<AStarNode> node = nData.Item1;
            float cost = nData.Item2;

            // fill the A* data for the node
            AStarNode nodeInfo = node.Info;
            nodeInfo.Heuristic = UnityEngine.Vector3.Distance(nodeInfo.position, destPosition);

            // update cost and parent only if we are in a better scenario
            if ((currNode.Info.Cost + cost) < nodeInfo.Cost)
            {
                nodeInfo.Cost = currNode.Info.Cost + cost; // TODO: it's better to use the distance between both nodes here instead of an explicit cost (check NOTE2 above)
                nodeInfo.Parent = currNode;
            }
        }

        private Node<AStarNode> SelectBestNeighbour(IEnumerable<Node<AStarNode>> neighbours)
        {
            var first = neighbours.First();

            return first != null ? neighbours.SelectBest((n1,n2) => n1.Info.F < n2.Info.F) : null;
        }

        private List<int> ComputeBackwardsPath(Node<AStarNode> destinationNode, Node<AStarNode> originNode)
        {
            List<int> path = new List<int> { destinationNode.Id };
            var currentNode = destinationNode;

            while (currentNode != originNode)
            {
                currentNode = currentNode.Info.Parent;
                path.Add(currentNode.Id);
            }

            return path;
        }
    }

    public static class AStarExtensionMethods
    {
        public static Node<AStarNode> SelectBest(this IEnumerable<Node<AStarNode>> nodes, Func<Node<AStarNode>, Node<AStarNode>, bool> predicate)
        {
            var best = nodes.First();
            foreach (var n in nodes)
            {
                if (predicate(n,best))
                {
                    best = n;
                }
            }

            return best;
        }
    }
}
