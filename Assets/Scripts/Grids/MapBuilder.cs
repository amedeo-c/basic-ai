using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellPathFinding
{
    public class MapBuilder : MonoBehaviour
    {
        public GameObject cellPrefab;
        public GameObject roadPrefab;

        public int shapeDimension;
        public int gridDepth;
        public float hSize;
        public float vSize;
        public float size;

        public bool flatTopPrefab;

        public Map map;

        private void OnValidate()
        {
            map = GetComponent<Map>();
        }
    }

    public enum MapShape
    {
        Square,
        Hexagon
    }
}
