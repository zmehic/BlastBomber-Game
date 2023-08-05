using System.Collections.Generic;

namespace BlastBomberV2.Core
{
    public class Node<T> where T : ILevelBlock
    {
        public int X { get; set; }
        public int Y { get; set; }

        public T Block { get; set; }

        public List<Node<T>> Neighbors { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;

            this.Neighbors = new List<Node<T>>();
            Block = default(T);
        }

        public Node(int x, int y, T block)
        {
            X = x;
            Y = y;
            Block = block;
            this.Neighbors = new List<Node<T>>();
        }

        public void AddNeighbor(Node<T> neighbor)
        {
            this.Neighbors.Add(neighbor);
        }

        
    }
}