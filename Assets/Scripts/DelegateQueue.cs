using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Queues
{
    public class DelegateQueue<T> : AbstractQueue<T>
    {
        System.Func<T, T, int> comparisonDelegate;

        public DelegateQueue(System.Func<T, T, int> comparisonDelegate)
        {
            this.comparisonDelegate = comparisonDelegate;
        }

        public override int ComparisonMethod(T a, T b)
        {
            return comparisonDelegate.Invoke(a, b);
        }
    }
}