using System;
using System.Collections.Generic;
using System.Linq;

namespace AITests.Pathfinding.Graphs
{
    public class Graph<T> : IGraph<T>
    {
        // the graph is represented as a dictionarly where the keys are the nodes and for each one the value is a list of pairs
        // for which the first element in the pair is the connected Node, and the second element is the cost of the connection
        private Dictionary<Node<T>, List<Tuple<Node<T>, float>>> nodes { get; set; }

        public Graph()
        {
            nodes = new Dictionary<Node<T>, List<Tuple<Node<T>, float>>>();
        }

        public void AddNode(Node<T> node)
        {
            if (nodes.ContainsKey(node))
            {
                return;
            }

            nodes.Add(node, null);
        }

        public void AddNodes(List<Node<T>> nodes)
        {
            foreach (var node in nodes)
            {
                AddNode(node);
            }
        }

        public void AddEdge(Node<T> nodeFrom, Node<T> nodeTo, float cost)
        {
            if (nodes.ContainsKey(nodeFrom))
            {
                List<Tuple<Node<T>, float>> edges;
                if (nodes.TryGetValue(nodeFrom, out edges))
                {
                    Tuple<Node<T>, float> edge = new Tuple<Node<T>, float>(nodeTo, cost);
                    List<Tuple<Node<T>, float>> edgeList = new List<Tuple<Node<T>, float>>();
                    edgeList.Add(edge);

                    if (edges == null)
                    {
                        nodes[nodeFrom] = edgeList;
                    }
                    else
                    {
                        nodes[nodeFrom].AddRange(edgeList);
                    }
                }
            }
        }

        public void AddBidirectionalEdge(Node<T> nodeFrom, Node<T> nodeTo, float cost)
        {
            AddEdge(nodeFrom, nodeTo, cost);
            AddEdge(nodeTo, nodeFrom, cost);
        }

        public void AddEdges(Node<T> node, List<Tuple<Node<T>, float>> edgesToAdd)
        {
            foreach (var edge in edgesToAdd)
            {
                AddEdge(node, edge.Item1, edge.Item2);
            }
        }

        public void AddBidirectionalEdges(Node<T> node, List<Tuple<Node<T>, float>> edgesToAdd)
        {
            foreach (var edge in edgesToAdd)
            {
                var nodeTo = edge.Item1;
                var cost = edge.Item2;
                
                AddEdge(node, nodeTo, cost);
                AddEdge(nodeTo, node, cost);
            }
        }

        public void RemoveNode(Node<T> node)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEdge(Node<T> node1, Node<T> node2)
        {
            throw new NotImplementedException();
        }

        public void RemoveBidirectionalEdge(Node<T> node1, Node<T> node2)
        {
            throw new NotImplementedException();
        }

        public Node<T> GetNode(int ID)
        {
            return nodes.Keys.Where(n => n.Id == ID).First();
        }
    }
}