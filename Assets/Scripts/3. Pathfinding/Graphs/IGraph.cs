using System;
using System.Collections.Generic;

namespace AITests.Pathfinding.Graphs
{
    public interface IGraph<T>
    {
        void AddNode(Node<T> node);
        void AddNodes(List<Node<T>> nodes);
        void AddEdge(Node<T> node1, Node<T> node2, float cost);
        void AddBidirectionalEdge(Node<T> nodeFrom, Node<T> nodeTo, float cost);
        void AddEdges(Node<T> node, List<Tuple<Node<T>, float>> edgesToAdd);
        void AddBidirectionalEdges(Node<T> node, List<Tuple<Node<T>, float>> edgesToAdd);

        void RemoveNode(Node<T> node);
        void RemoveEdge(Node<T> node1, Node<T> node2);
        void RemoveBidirectionalEdge(Node<T> node1, Node<T> node2);

        Node<T> GetNode(int ID);
        List<Node<T>> GetNodes();
        List<Tuple<Node<T>, float>> GetAdjacentNodes(Node<T> node);
    }
}


