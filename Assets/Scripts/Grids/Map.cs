using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using UnityEngine.AI;

namespace CellPathFinding
{
    public class Map : MonoBehaviour
    {
        public static Cell[,] cells; // to be initialized on play since not serializable.

        private void Start()
        {
            SetupCellsArray();
        }

        public void SetupCellsArray()
        {
            IEnumerable<Cell> sceneCells = FindObjectsOfType<Cell>(true);

            int maxJ = sceneCells.Max(item => item.j);
            int maxI = sceneCells.Max(item => item.i);
            cells = new Cell[maxJ + 1, maxI + 1];

            foreach (Cell c in sceneCells)
            {
                cells[c.j, c.i] = c;
            }
        }
    }

}