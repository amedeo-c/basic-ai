using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Queues
{
    public class ComparableQueue<T> : AbstractQueue<T> where T : IComparable<T>
    {
        public override int ComparisonMethod(T a, T b)
        {
            return a.CompareTo(b);
        }
    }
}
