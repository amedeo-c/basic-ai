using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellPathFinding
{
    [System.Serializable]
    public class MapPath
    {
        public List<Cell> cells;

        public MapPath(List<Cell> cells)
        {
            if (cells == null)
            {
                Debug.LogError("creating path with null cells");
            }
            this.cells = cells;
        }

        public int Length
        {
            get
            {
                return cells.Count;
            }
        }

        public Cell Source
        {
            get
            {
                return cells[0];
            }
        }

        public Cell Destination
        {
            get
            {
                return cells[Length - 1];
            }
        }

        public Cell GetCell(int index)
        {
            if (index >= 0 && index < cells.Count)
            {
                return cells[index];
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            string s = "";

            foreach (Cell c in cells)
            {
                s += c.ToString() + ",  ";
            }

            return s;
        }

        public void Draw(float duration = 0f)
        {
            if (cells == null)
            {
                return;
            }

            Vector3 offset = Vector3.up * 0.5f;

            for (int i = 0; i < cells.Count - 1; i++)
            {
                if (duration > 0.1f)
                {
                    Debug.DrawLine(cells[i].transform.position + offset, cells[i + 1].transform.position + offset, Color.red, duration);
                }
                else
                {
                    Debug.DrawLine(cells[i].transform.position + offset, cells[i + 1].transform.position + offset, Color.red);
                }
            }
        }
    }
}


