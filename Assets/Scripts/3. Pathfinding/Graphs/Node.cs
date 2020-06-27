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

        public Node(int id, T info)
        {
            this.Id = id;
            this.Info = info;
        }

        public override bool Equals(object n)
        {
            return IsEqual(n as Node<T>);
        }

        private bool IsEqual(Node<T> n)
        {
            return this.Id == n.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = 227837733;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Node<T> node1, Node<T> node2)
        {
            if (object.ReferenceEquals(node1, null)) return object.ReferenceEquals(node2, null);
            if (object.ReferenceEquals(node2, null)) return object.ReferenceEquals(node1, null);

            return node1.IsEqual(node2);
        }

        public static bool operator !=(Node<T> node1, Node<T> node2)
        {
            if (object.ReferenceEquals(node1, null)) return !object.ReferenceEquals(node2, null);
            if (object.ReferenceEquals(node2, null)) return !object.ReferenceEquals(node1, null);

            return !node1.IsEqual(node2);
        }
    }
}
