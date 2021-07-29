using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellPathFinding
{
    public abstract class Cell : MonoBehaviour
    {
        public static int shapeDimension;

        [Header("Coordinates")]
        public int j;
        public int i;

        public abstract Cell GetNeighbor(int index);

        public abstract Edge GetEdge(int index);

        public abstract Vector3 GetPosition();

        public static CoordinatePair AdjacentPair(CoordinatePair pair, int index)
        {
            switch (shapeDimension)
            {
                case 4:

            }
        }


    }

    public struct CoordinatePair
    {
        public int j;
        public int i;

        public CoordinatePair(int j, int i)
        {
            this.j = j;
            this.i = i;
        }
    }
}
