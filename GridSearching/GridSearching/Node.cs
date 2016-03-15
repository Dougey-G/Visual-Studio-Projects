using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

namespace GridSearching
{
    class Node
    {
        List<Edge> neighbors = new List<Edge>();
        Edge back = null;
        bool isVisited = false;
        bool isObstacle = false;
        GridCell gridCell;
        float distance = 0;
        Vector2 position;

        public Node(GridCell gridCell, int x, int y)
        {
            this.gridCell = gridCell;
            position = new Vector2(x, y);
            if (gridCell.IsBlocked)
            {
                isObstacle = true;
            }
        }

        public Vector2 Position
        {
            get { return position; }
        }


        public List<Edge> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public Edge BackNode
        {
            get { return back; }
            set
            {
                back = value;
                distance = back.GetNeighbor(this).distance + back.Length;
            }
        }

        public bool IsObstacle
        {
            get { return isObstacle; }
            set { isObstacle = value; }
        }

        public void UpdateObstacle()
        {
            if (gridCell.IsBlocked)
            {
                isObstacle = true;
            }
            else
            {
                isObstacle = false;
            }
        }

        public float GetDistance
        {
            get { return distance; }
        }

        public Color ChangeColor
        {
            set { gridCell.ChangeColor = value; }
        }


        public bool IsVisited
        {
            get { return isVisited; }
            set
            {
                isVisited = value;
                gridCell.IsVisited = isVisited;
            }
        }
    }
}
