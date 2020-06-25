namespace AITests.Pathfinding.Graphs.AStar
{
    public class AStarNode
    {
        public UnityEngine.Vector3 position { get; private set; }
        public float Heuristic { get; set; }
        public float Cost { get; set; }
        public float F
        {
            get
            {
                return Heuristic + Cost;
            }
        }

        public int Parent { get; set; }

        public AStarNode(UnityEngine.Vector3 position)
        {
            this.position = position;
        }
    }
}
