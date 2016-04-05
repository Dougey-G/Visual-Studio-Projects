using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SimpleRTS
{
    class Graph
    {
        #region fields

        Node[,] nodeGrid;

        int width, height;

        //Variables for the search functions
        Node curNode;
        Stack<Vector2> targetPath;
        PriorityQueue nodeQueue;
        float topPriority;

        #endregion

        public Graph(int width, int height)
        {
            this.width = width;
            this.height = height;

            //Initializes the node array
            nodeGrid = new Node[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodeGrid[x, y] = new Node(new Vector2(x, y));
                }
            }
            
            //Sets the neighbors of each node
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //if node has 8 neighbors
                    if (x != 0 && x != width - 1 && y != 0 && y != height - 1)
                    {
                        nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1),
                        new Edge(nodeGrid[x, y], nodeGrid[x - 1, y - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y + 1], (float)Math.Sqrt(2)),};
                    }
                    //if node has 3 neighbors
                    else if ((x == 0 || x == width - 1) && (y == 0 || y == height - 1))
                    {
                        if (x == 0 && y == 0)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y + 1], (float)Math.Sqrt(2)) };
                        }
                        else if (x == 0 && y == height - 1)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y - 1], (float)Math.Sqrt(2)) };
                        }
                        else if (x == width - 1 && y == 0)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y + 1], (float)Math.Sqrt(2)) };
                        }
                        else if (x == width - 1 && y == height - 1)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y - 1], (float)Math.Sqrt(2)) };
                        }
                    }
                    //if node has 5 neighbors
                    else
                    {
                        if (x == 0)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1),
                            new Edge(nodeGrid[x, y], nodeGrid[x + 1, y + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y - 1], (float)Math.Sqrt(2))};

                        }
                        else if (x == width - 1)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1),
                            new Edge(nodeGrid[x, y], nodeGrid[x - 1, y - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y + 1], (float)Math.Sqrt(2))};
                        }
                        else if (y == 0)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x, y + 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1),
                            new Edge(nodeGrid[x, y], nodeGrid[x - 1, y + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y + 1], (float)Math.Sqrt(2))};
                        }
                        else if (y == height - 1)
                        {
                            nodeGrid[x, y].Neighbors = new List<Edge> { new Edge(nodeGrid[x, y], nodeGrid[x + 1, y], 1), new Edge(nodeGrid[x, y], nodeGrid[x, y - 1], 1), new Edge(nodeGrid[x, y], nodeGrid[x - 1, y], 1),
                            new Edge(nodeGrid[x, y], nodeGrid[x - 1, y - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[x, y], nodeGrid[x + 1, y - 1], (float)Math.Sqrt(2))};
                        }
                    }
                }
            }
        }

            #region public methods

        /// <summary>
        /// Uses A* to find the fastest path to the goal
        /// </summary>
        /// <param name="start">Start location of path</param>
        /// <param name="finish">Finish location of path</param>
        /// <returns>Queue of locations which is the path</returns>
        public Stack<Vector2> ComputePath(Vector2 start, Vector2 finish)
        {
            //Initialize our variables
            start = new Vector2(start.X / (Game1.WINDOW_WIDTH / width), start.Y / (Game1.WINDOW_HEIGHT / height));
            finish = new Vector2(finish.X / (Game1.WINDOW_WIDTH / width), finish.Y / (Game1.WINDOW_HEIGHT / height));
            nodeQueue = new PriorityQueue();
            targetPath = new Stack<Vector2>();
            nodeQueue.Enqueue(nodeGrid[(int)start.X, (int)start.Y], 0);
            nodeGrid[(int)start.X, (int)start.Y].IsVisited = true;

            while (nodeQueue.Count > 0)
            {
                nodeQueue.Dequeue(out curNode, out topPriority);

                //If we have found the target node
                if (curNode == nodeGrid[(int)finish.X, (int)finish.Y])
                {
                    targetPath.Push(new Vector2(curNode.Position.X * (Game1.WINDOW_WIDTH / width), curNode.Position.Y * (Game1.WINDOW_HEIGHT / height)));

                    //Loop back through the backnodes to find the fastest path and add them to the targetPath variable
                    while (curNode.BackNode != null)
                    {
                        curNode = curNode.BackNode.GetNeighbor(curNode);
                        targetPath.Push(new Vector2(curNode.Position.X * (Game1.WINDOW_WIDTH / width), curNode.Position.Y * (Game1.WINDOW_HEIGHT / height)));
                    }
                    ResetGraph();
                    return targetPath;
                }

                //Look through each neighbor to find who we have not checked yet
                foreach (Edge edge in curNode.Neighbors)
                {
                    float priority = curNode.Length + edge.Length + Vector2.Distance(edge.GetNeighbor(curNode).Position, nodeGrid[(int)finish.X, (int)finish.Y].Position);
                    if (!edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsBlocked)
                    {
                        edge.GetNeighbor(curNode).IsVisited = true;
                        edge.GetNeighbor(curNode).BackNode = edge;
                        nodeQueue.Enqueue(edge.GetNeighbor(curNode), priority);
                        edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                    }
                    else if (edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsBlocked)
                    {
                        if (edge.GetNeighbor(curNode).Length > curNode.Length + edge.Length)
                        {
                            nodeQueue.ReplacePriority(edge.GetNeighbor(curNode), priority);
                            edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                            edge.GetNeighbor(curNode).BackNode = edge;
                        }
                    }
                }
            }

            //if unit cannot find path, dont move him
            targetPath.Clear();
            ResetGraph();
            return targetPath;
        }

        public void ResetGraph()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodeGrid[x, y].Length = 0;
                    nodeGrid[x, y].BackNode = null;
                    nodeGrid[x, y].IsVisited = false;
                }
            }
        }

        /// <summary>
        /// Blocks a specific node so that other units cannot traverse it.
        /// </summary>
        /// <param name="location">the location of the node to block</param>
        public void SetNodeBlocked(Vector2 location)
        {
            nodeGrid[(int)location.X / (Game1.WINDOW_WIDTH / width), (int)location.Y / (Game1.WINDOW_HEIGHT / height)].IsBlocked = true;
        }

        /// <summary>
        /// Unblock a specific node so units can traverse it.
        /// </summary>
        /// <param name="location">the location of the node to unblock</param>
        public void UnblockNode(Vector2 location)
        {
            nodeGrid[(int)location.X / (Game1.WINDOW_WIDTH / width), (int)location.Y / (Game1.WINDOW_HEIGHT / height)].IsBlocked = false;
        }

        public bool IsNodeBlocked(Vector2 location)
        {
            return nodeGrid[(int)location.X / (Game1.WINDOW_WIDTH / width), (int)location.Y / (Game1.WINDOW_HEIGHT / height)].IsBlocked;

        }

        public Node GetNode(Vector2 location)
        {
            return nodeGrid[(int)location.X / (Game1.WINDOW_WIDTH / width), (int)location.Y / (Game1.WINDOW_HEIGHT / height)];
        }
            #endregion
    }




    class PriorityQueue
    {
        #region fields
        List<Node> values = new List<Node>();
        List<float> priorities = new List<float>();
        #endregion

        public PriorityQueue() { }


        #region Properties
        public int Count
        {
            get { return values.Count; }
        }
        #endregion

        #region public methods
        public void Enqueue(Node value, float priority)
        {
            values.Add(value);
            priorities.Add(priority);
        }

        /// <summary>
        /// Searches through the list to find the node with the highest priority and returns it/
        /// </summary>
        /// <param name="topValue">Node with highest priority</param>
        /// <param name="topPriority">Priority value of that node</param>
        public void Dequeue(out Node topValue, out float topPriority)
        {
            int bestIndex = 0;
            float bestPriority = priorities[0];
            for (int i = 1; i < priorities.Count; i++)
            {
                if (bestPriority > priorities[i])
                {
                    bestPriority = priorities[i];
                    bestIndex = i;
                }
            }


            topValue = values[bestIndex];
            topPriority = bestPriority;

            values.RemoveAt(bestIndex);
            priorities.RemoveAt(bestIndex);
        }

        /// <summary>
        /// Returns the priorty of a node
        /// </summary>
        /// <param name="node">Node to get the priority of</param>
        /// <returns>Priorty of the node</returns>
        public float GetPriority(Node node)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == node)
                {
                    return priorities[i];
                }
            }
            return 0;
        }

        /// <summary>
        /// Replaces the priorty of a node if a shorter path is found
        /// </summary>
        /// <param name="value">Node to replace the priority of</param>
        /// <param name="priority">New priority to set to the node</param>
        public void ReplacePriority(Node value, float priority)
        {
            int index = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == value)
                {
                    index = i;
                }
            }

            values.RemoveAt(index);
            priorities.RemoveAt(index);

            values.Add(value);
            priorities.Add(priority);
        }

        /// <summary>
        /// Clears the entire lists so they can be used again for another search
        /// </summary>
        public void Clear()
        {
            values.Clear();
            priorities.Clear();
        }
        #endregion
    }
}


