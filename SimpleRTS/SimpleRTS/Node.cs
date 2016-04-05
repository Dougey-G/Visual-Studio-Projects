using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleRTS
{
    class Node
    {
        #region fields

        Vector2 position;
        float length = 0;
        float distance = float.PositiveInfinity;
        bool isVisited = false;
        bool isBlocked = false;
        List<Edge> neighbors = new List<Edge>();
        Edge backPointer = null;

        #endregion

        public Node(Vector2 position)
        {
            this.position = position;
        }

        #region properties

        public Vector2 Position
        {
            get { return position; }
        }

        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public bool IsVisited
        {
            get { return isVisited; }
            set { isVisited = value; }
        }

        public bool IsBlocked
        {
            get { return isBlocked; }
            set { isBlocked = value; }
        }

        public List<Edge> Neighbors
        {
            get { return neighbors; }
            set { neighbors = value; }
        }

        public Edge BackNode
        {
            get { return backPointer; }
            set { backPointer = value; }
        }

        #endregion

    }
}
