using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using Microsoft.Xna.Framework.Content;

namespace GridSearching
{
    class Graph
    {
        enum SearchType { BREADTH, DIJKSTRA, ASTAR }
        SearchType searchType;
        GridCell[,] gridCell;
        Node[,] nodeGrid;
        int width, height;
        Timer timer;

        //stuff for the search
        Vector2 start;
        Vector2 end;
        Node curNode;
        Queue<Node> nodeQueue;
        bool hasResult = false;

        //stuff for priority queue
        float topPriority;
        PriorityQueue priorityQueue;

        public Graph(GridCell[,] gridCell, int width, int height)
        {
            this.gridCell = gridCell;
            this.width = width;
            this.height = height;
            nodeGrid = new Node[width, height];
            searchType = SearchType.BREADTH;

            //Initializes all of the nodes
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!gridCell[i, j].IsBlocked)
                        gridCell[i, j].ChangeColor = Color.CornflowerBlue;
                    nodeGrid[i, j] = new Node(gridCell[i, j], i, j);
                }
            }

            //Sets neighboring nodes for each node
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //if node has four neighbors
                    if (i != 0 && i != width - 1 && j != 0 && j != height - 1)
                    {
                        nodeGrid[i, j].Neighbors = new List<Edge> {new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                    }
                    //if node has two neighbors
                    else if ((i == 0 || i == width - 1) && (j == 0 || j == height - 1))
                    {
                        if (i == 0 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1) };
                        }
                        else if (i == 0 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1) };
                        }
                        else if (i == width - 1 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                        }
                        else if (i == width - 1 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                        }
                    }
                    //if node has three neighbors
                    else
                    {
                        if (i == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1) };
                            
                        }
                        else if (i == width - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                        }
                        else if (j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                        }
                        else if (j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1) };
                        }
                    }
                }
            }
        }

        public Graph(GridCell[,] gridCell, int width, int height, bool isDijkstra)
        {
            this.gridCell = gridCell;
            this.width = width;
            this.height = height;
            nodeGrid = new Node[width, height];
            if (isDijkstra)
            {
                searchType = SearchType.DIJKSTRA;
            }
            else
            {
                searchType = SearchType.ASTAR;
            }

            //Initializes all of the nodes
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!gridCell[i, j].IsBlocked)
                        gridCell[i, j].ChangeColor = Color.CornflowerBlue;
                    nodeGrid[i, j] = new Node(gridCell[i, j], i, j);
                }
            }

            //Sets neighboring nodes for each node
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    //if node has 8 neighbors
                    if (i != 0 && i != width - 1 && j != 0 && j != height - 1)
                    {
                        nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1),
                        new Edge(nodeGrid[i, j], nodeGrid[i - 1, j - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j + 1], (float)Math.Sqrt(2)),};
                    }
                    //if node has 3 neighbors
                    else if ((i == 0 || i == width - 1) && (j == 0 || j == height - 1))
                    {
                        if (i == 0 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j + 1], (float)Math.Sqrt(2)) };
                        }
                        else if (i == 0 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j - 1], (float)Math.Sqrt(2)) };
                        }
                        else if (i == width - 1 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j + 1], (float)Math.Sqrt(2)) };
                        }
                        else if (i == width - 1 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j - 1], (float)Math.Sqrt(2)) };
                        }
                    }
                    //if node has 5 neighbors
                    else
                    {
                        if (i == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1),
                            new Edge(nodeGrid[i, j], nodeGrid[i + 1, j + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j - 1], (float)Math.Sqrt(2))};

                        }
                        else if (i == width - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1),
                            new Edge(nodeGrid[i, j], nodeGrid[i - 1, j - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j + 1], (float)Math.Sqrt(2))};
                        }
                        else if (j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i, j + 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1),
                            new Edge(nodeGrid[i, j], nodeGrid[i - 1, j + 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j + 1], (float)Math.Sqrt(2))};
                        }
                        else if (j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Edge> { new Edge(nodeGrid[i, j], nodeGrid[i + 1, j], 1), new Edge(nodeGrid[i, j], nodeGrid[i, j - 1], 1), new Edge(nodeGrid[i, j], nodeGrid[i - 1, j], 1),
                            new Edge(nodeGrid[i, j], nodeGrid[i - 1, j - 1], (float)Math.Sqrt(2)), new Edge(nodeGrid[i, j], nodeGrid[i + 1, j - 1], (float)Math.Sqrt(2))};
                        }
                    }
                }
            }
        }

        public Vector2 curNodePosition
        {
            get { return curNode.Position; }

        }

        public void Update(GameTime gameTime)
        {
            if (timer.IsFinished && !hasResult && searchType == SearchType.BREADTH)
            {
                timer.Reset();
                BreadthFirstSearch();
            }
            else if (timer.IsFinished && !hasResult && searchType == SearchType.ASTAR)
            {
                timer.Reset();
                AStarSearch();
            }
            else if (timer.IsFinished && !hasResult && searchType == SearchType.DIJKSTRA)
            {
                timer.Reset();
                DijkstraSearch();
            }
            timer.Update(gameTime);
        }

        public void AStarSearchInitialize(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            timer = new Timer(25);
            timer.Reset();
            priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue(nodeGrid[(int)start.X, (int)start.Y], 0);
            //priorityQueue.Dequeue(out topValue, out topPriority);
            nodeGrid[(int)start.X, (int)start.Y].ChangeColor = Color.Red;
            nodeGrid[(int)start.X, (int)start.Y].IsVisited = true;
            nodeGrid[(int)end.X, (int)end.Y].ChangeColor = Color.Green;
        }

        public void AStarSearch()
        {
            //Set the distance to all nodes to infinity (i.e. Int32.MaxValue)
            if (priorityQueue.Count > 0)
            {
                priorityQueue.Dequeue(out curNode, out topPriority);
                curNode.ChangeColor = Color.Yellow;

                //if we have found the target node
                if (curNode == nodeGrid[(int)end.X, (int)end.Y])
                {
                    hasResult = true;
                    //loop back through the backnodes to find the fastest path
                    while (curNode.BackNode != null)
                    {
                        curNode = curNode.BackNode.GetNeighbor(curNode);
                        curNode.ChangeColor = Color.HotPink;
                    }
                    return;
                }

                //look through each neighbor to find who we have not checked yet.
                foreach (Edge edge in curNode.Neighbors)
                {
                    if (!edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsObstacle)
                    {
                        edge.GetNeighbor(curNode).IsVisited = true;
                        edge.GetNeighbor(curNode).BackNode = edge; //sets distance within this property: distance = curnode.GetDistance + 1;
                        priorityQueue.Enqueue(edge.GetNeighbor(curNode), curNode.Length + edge.Length + Vector2.Distance(edge.GetNeighbor(curNode).Position, nodeGrid[(int)end.X, (int)end.Y].Position));
                        edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                    }
                    else if (edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsObstacle)
                    {
                        if (edge.GetNeighbor(curNode).Length  > curNode.Length + edge.Length)
                        {
                            priorityQueue.ReplacePriority(edge.GetNeighbor(curNode), curNode.Length + edge.Length + Vector2.Distance(edge.GetNeighbor(curNode).Position, nodeGrid[(int)end.X, (int)end.Y].Position));
                            edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                            edge.GetNeighbor(curNode).BackNode = edge;
                        }
                    }
                }
            }
            else
            {
                //return error
            }
        }

        public void DijkstraSearchInitialize(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            timer = new Timer(25);
            timer.Reset();
            priorityQueue = new PriorityQueue();
            priorityQueue.Enqueue(nodeGrid[(int)start.X, (int)start.Y], 0);
            //priorityQueue.Dequeue(out topValue, out topPriority);
            nodeGrid[(int)start.X, (int)start.Y].ChangeColor = Color.Red;
            nodeGrid[(int)start.X, (int)start.Y].IsVisited = true;
            nodeGrid[(int)end.X, (int)end.Y].ChangeColor = Color.Green;
        }

        public void DijkstraSearch()
        {
            //Set the distance to all nodes to infinity (i.e. Int32.MaxValue)
            if (priorityQueue.Count > 0)
            {
                priorityQueue.Dequeue(out curNode, out topPriority);
                curNode.ChangeColor = Color.Yellow;



                //look through each neighbor to find who we have not checked yet.
                foreach (Edge edge in curNode.Neighbors)
                {
                    if (!edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsObstacle)
                    {
                        //if we have found the target node
                        if (edge.GetNeighbor(curNode) == nodeGrid[(int)end.X, (int)end.Y])
                        {
                            hasResult = true;
                            nodeGrid[(int)end.X, (int)end.Y].BackNode = edge;
                            curNode = nodeGrid[(int)end.X, (int)end.Y];

                            //loop back through the backnodes to find the fastest path
                            while (curNode.BackNode != null)
                            {
                                curNode = curNode.BackNode.GetNeighbor(curNode);
                                curNode.ChangeColor = Color.HotPink;
                            }
                            return;
                        }

                        edge.GetNeighbor(curNode).IsVisited = true;
                        edge.GetNeighbor(curNode).BackNode = edge; //sets distance within this property: distance = curnode.GetDistance + 1;
                        priorityQueue.Enqueue(edge.GetNeighbor(curNode), curNode.Length + edge.Length);
                        edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                    }
                    else if (edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsObstacle)
                    {
                        if (edge.GetNeighbor(curNode).Length > curNode.Length + edge.Length)
                        {
                            priorityQueue.ReplacePriority(edge.GetNeighbor(curNode), curNode.Length + edge.Length);
                            edge.GetNeighbor(curNode).Length = curNode.Length + edge.Length;
                            edge.GetNeighbor(curNode).BackNode = edge;
                        }
                    }
                }
            }
            else
            {
                //return error
            }
        }

        public void BreadthFirstSearchInitialize(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            timer = new Timer(25);
            timer.Reset();
            nodeQueue = new Queue<Node>();
            nodeQueue.Enqueue(nodeGrid[(int)start.X, (int)start.Y]);
            nodeGrid[(int)start.X, (int)start.Y].ChangeColor = Color.Red;
            nodeGrid[(int)start.X, (int)start.Y].IsVisited = true;
            nodeGrid[(int)end.X, (int)end.Y].ChangeColor = Color.Green;
        }

        public void BreadthFirstSearch()
        {
            //Set the distance to all nodes to infinity (i.e. Int32.MaxValue)
            if (nodeQueue.Count > 0)
            {
                curNode = nodeQueue.Dequeue();
                curNode.ChangeColor = Color.Yellow;

                //if we have found the target node
                if (curNode == nodeGrid[(int)end.X, (int)end.Y])
                {
                    hasResult = true;
                    //loop back through the backnodes to find the fastest path
                    while (curNode.BackNode != null)
                    {
                        curNode = curNode.BackNode.GetNeighbor(curNode);
                        curNode.ChangeColor = Color.HotPink;
                    }
                    return;
                }

                //look through each neighbor to find who we have not checked yet.
                foreach (Edge edge in curNode.Neighbors)
                {
                    if (!edge.GetNeighbor(curNode).IsVisited && !edge.GetNeighbor(curNode).IsObstacle)
                    {
                        edge.GetNeighbor(curNode).IsVisited = true;
                        edge.GetNeighbor(curNode).BackNode = edge; //sets distance within this property: distance = curnode.GetDistance + 1;
                        nodeQueue.Enqueue(edge.GetNeighbor(curNode));
                    }
                }
            }
            else
            {
                //return error
            }
        }

        public void UpdateObstacles(Vector2 position)
        {
            nodeGrid[(int)position.X, (int)position.Y].UpdateObstacle();
        }
    }
}
