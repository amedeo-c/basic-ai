using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Queues
{
    public abstract class AbstractQueue<T>
    {
        private List<T> data = new List<T>();

        public abstract int ComparisonMethod(T a, T b);

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1;
            while (ci > 0)
            {
                int pi = (ci - 1) / 2;
                if(ComparisonMethod(data[ci], data[pi]) >= 0)
                    break;
                T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            Debug.Assert(data.Count > 0);
            int li = data.Count - 1;
            T frontItem = data[0];
            data[0] = data[li];
            data.RemoveAt(li);

            --li;
            int pi = 0;
            while (true)
            {
                int ci = pi * 2 + 1;
                if (ci > li) break;
                int rc = ci + 1;
                if (rc <= li && ComparisonMethod(data[rc], data[ci]) < 0)

                        ci = rc;
                if (ComparisonMethod(data[pi], data[ci]) <= 0) break;

                T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp;
                pi = ci;
            }
            return frontItem;
        }

        public int Count()
        {
            return data.Count;
        }

        public bool Empty()
        {
            if (data.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public T Peek()
        {
            T frontItem = data[0];
            return frontItem;
        }

        public T LastOut()
        {
            T lastItem = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            return lastItem;
        }

        public bool Contains(T itemToSearch)
        {
            for (int i = 0; i < Count(); i++)
            {
                if (itemToSearch.Equals(data[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsConsistent()
        {
            if (data.Count == 0) return true;
            int li = data.Count - 1;
            for (int pi = 0; pi < data.Count; ++pi)
            {
                int lci = 2 * pi + 1;
                int rci = 2 * pi + 2;
                if (lci <= li && ComparisonMethod(data[pi], data[lci]) > 0) return false;
                if (rci <= li && ComparisonMethod(data[pi], data[rci]) > 0) return false;
            }
            return true;
        }
    }
}
