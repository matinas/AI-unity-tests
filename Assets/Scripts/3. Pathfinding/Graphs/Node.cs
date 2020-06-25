namespace AITests.Pathfinding.Graphs
{
    public class Node<T>
    {
        public int Id { get; private set; }
        public T Info { get; set; }

        public Node(int Id)
        {
            this.Id = Id;
        }

        public Node(int Id, T info)
        {
            this.Id = Id;
            this.Info = Info;
        }
    }
}
