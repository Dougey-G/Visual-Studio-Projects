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

        public Graph(GridCell[,] gridCell, int width, int height)
        {
            this.gridCell = gridCell;
            this.width = width;
            this.height = height;
            nodeGrid = new Node[width, height];

            //Initializes all of the nodes
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    gridCell[i, j].ChangeColor = Color.CornflowerBlue;
                    nodeGrid[i, j] = new Node(gridCell[i, j]);
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
                        nodeGrid[i, j].Neighbors = new List<Node> {nodeGrid[i, j + 1], nodeGrid[i + 1, j], nodeGrid[i, j - 1], nodeGrid[i - 1, j] };
                    }
                    //if node has two neighbors
                    else if ((i == 0 || i == width - 1) && (j == 0 || j == height - 1))
                    {
                        if (i == 0 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j + 1], nodeGrid[i + 1, j] };
                        }
                        else if (i == 0 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i + 1, j], nodeGrid[i, j - 1] };
                        }
                        else if (i == width - 1 && j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j + 1], nodeGrid[i - 1, j] };
                        }
                        else if (i == width - 1 && j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j - 1], nodeGrid[i - 1, j] };
                        }
                    }
                    //if node has three neighbors
                    else
                    {
                        if (i == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j + 1], nodeGrid[i + 1, j], nodeGrid[i, j - 1] };
                            
                        }
                        else if (i == width - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j + 1], nodeGrid[i, j - 1], nodeGrid[i - 1, j] };
                        }
                        else if (j == 0)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i, j + 1], nodeGrid[i + 1, j], nodeGrid[i - 1, j] };
                        }
                        else if (j == height - 1)
                        {
                            nodeGrid[i, j].Neighbors = new List<Node> { nodeGrid[i + 1, j], nodeGrid[i, j - 1], nodeGrid[i - 1, j] };
                        }
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (timer.IsFinished && !hasResult)
            {
                timer.Reset();
                BreadthFirstSearch();
            }
            timer.Update(gameTime);
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
                        curNode = curNode.BackNode;
                        curNode.ChangeColor = Color.HotPink;
                    }
                    return;
                }

                //look through each neighbor to find who we have not checked yet.
                foreach (Node node in curNode.Neighbors)
                {
                    if (!node.IsVisited)
                    {
                        node.IsVisited = true;
                        node.BackNode = curNode; //sets distance within this property: distance = curnode.GetDistance + 1;
                        nodeQueue.Enqueue(node);
                    }
                }
            }
            else
            {
                //return error
            }
        }
    }
}
