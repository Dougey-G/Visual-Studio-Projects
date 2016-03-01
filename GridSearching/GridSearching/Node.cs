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
        List<Node> neighbors = new List<Node>();
        Node back = null;
        bool isVisited = false;
        GridCell gridCell;
        int distance = 0;

        public Node(GridCell gridCell)
        {
            this.gridCell = gridCell;
        }

        public List<Node> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public Node BackNode
        {
            get { return back; }
            set
            {
                back = value;
                distance = back.GetDistance + 1;
            }
        }

        public int GetDistance
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
