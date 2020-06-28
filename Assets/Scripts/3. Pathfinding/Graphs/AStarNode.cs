namespace AITests.Pathfinding.Graphs.AStar
{
    public class AStarNode
    {
        public float Heuristic { get; set; }
        public float Cost { get; set; }
        public float F
        {
            get
            {
                return Heuristic + Cost;
            }
        }

        public Node<AStarNode> Parent { get; set; }

        public UnityEngine.Vector3 position { get; set; }

        public AStarNode()
        {
            Reset();
        }

        public AStarNode(UnityEngine.Vector3 position) : this()
        {
            this.position = position;
        }

        public void Reset()
        {
            this.Heuristic = 0.0f;
            this.Cost = float.MaxValue;
            this.Parent = null;
        }
    }
}