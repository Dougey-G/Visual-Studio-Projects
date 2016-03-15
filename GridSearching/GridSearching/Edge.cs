using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridSearching
{

    class Edge
    {
        Node node1, node2;
        float length;

        public Edge(Node node1, Node node2, float length)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.length = length;
        }

        public float Length
        {
            get { return length; }
        }


        public Node GetNeighbor(Node node)
        {
            if (node == node1)
            {
                return node2;
            }
            else if (node == node2)
            {
                return node1;
            }
            else
            {
                return null;
            }
        }

    }
}
