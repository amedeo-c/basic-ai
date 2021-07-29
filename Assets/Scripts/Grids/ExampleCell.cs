using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellPathFinding
{
    public class ExampleCell : Cell
    {
        [Header("Adjacencies")]
        public Cell[] neighbors;
        public Edge[] edges;

        void Awake()
        {
            neighbors = new Cell[shapeDimension];
            edges = new Edge[shapeDimension];
        }

        #region Cell Methods Overrides

        public override Cell GetNeighbor(int index)
        {
            return neighbors[index];
        }

        public override Edge GetEdge(int index)
        {
            return edges[index];
        }

        public override Vector3 GetPosition()
        {
            return transform.position;
        }

        #endregion
    }
}
