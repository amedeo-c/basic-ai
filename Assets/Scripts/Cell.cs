using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellPathFinding
{
    public abstract class Cell : MonoBehaviour
    {
        public static int shapeDimension;

        public abstract Cell GetNeighbor(int index);

        public abstract Edge GetEdge(int index);

        public abstract Vector3 GetPosition();
    }
}
