using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GridSearching
{
    class PriorityQueue
    {
        List<Node> values = new List<Node>();
        List<float> priorities = new List<float>();

        public PriorityQueue()
        {


        }

        public int Count
        {
            get { return values.Count; }
        }

        public void Enqueue(Node value, float priority)
        {
            values.Add(value);
            priorities.Add(priority);
        }

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

        public float GetPriority(Node node)
        {
            for(int i = 0; i < values.Count; i++)
            {
                if (values[i] == node)
                {
                    return priorities[i];
                }
            }
            return 0;
        }

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

        public void Clear()
        {
            values.Clear();
            priorities.Clear();
        }
    }
}
