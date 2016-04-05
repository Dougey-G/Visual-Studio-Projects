using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleRTS
{
    class Edge
    {
        #region fields

        Node first, second;
        float length;

        #endregion

        /// <summary>
        /// Constructor for the Edge.
        /// </summary>
        /// <param name="first">The first end of the edge.</param>
        /// <param name="second">The second end of the edge.</param>
        /// <param name="length">The distance between the edges.</param>
        public Edge(Node first, Node second, float length)
        {
            this.first = first;
            this.second = second;
            this.length = length;
        }

        #region properties

        /// <summary>
        /// Returns the distance of the edge.
        /// </summary>
        public float Length
        {
            get { return length; }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Compares the two nodes and sends back the nodes neighbor.
        /// </summary>
        /// <param name="node">the node we want to fight the neighbor of</param>
        /// <returns>the neighbor</returns>
        public Node GetNeighbor(Node node)
        {
            if (node == first)
            {
                return second;
            }
            else if (node == second)
            {
                return first;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
