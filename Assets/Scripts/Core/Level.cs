using UnityEngine;

namespace BlastBomberV2.Core
{
    public class Level<T> where T:ILevelBlock
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int NodeSize { get; set; }
        public Node<T>[,] Grid { get; private set; }

        public Level(int width, int height, int nodeSize=1)
        {
            Width = width;
            Height = height;
            NodeSize = nodeSize;

            Grid = new Node<T>[width, height];
        }

        public Vector3 NodeToWorldPosition(int x, int y)=>
            new Vector3(x, -0.5f, y) * NodeSize;

        public Vector2Int WorldToNodePosition(Vector3 position) =>
            new Vector2Int(Mathf.RoundToInt(position.x / NodeSize), Mathf.RoundToInt(position.z / NodeSize));

        public void AddNode(int x, int y, T block) => Grid[x, y]=new Node<T>(x,y,block);
        
        public void DetectNeighbors(Node<T> node)
        {
            if (Grid[node.X - 1, node.Y] != null)
            {
                node.AddNeighbor(Grid[node.X - 1, node.Y]);
            }

            if (Grid[node.X + 1, node.Y] != null)
            {
                node.AddNeighbor(Grid[node.X + 1, node.Y]);
            }

            if (Grid[node.X,node.Y-1]!=null)
            {
                node.AddNeighbor(Grid[node.X,node.Y-1]);
            }

            if (Grid[node.X, node.Y + 1] != null)
            {
                node.AddNeighbor(Grid[node.X, node.Y + 1]);
            }
        }
    }
}
