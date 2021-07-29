using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Linq;
using UnityEditor;

namespace CellPathFinding
{
    [CustomEditor(typeof(MapBuilder))]
    public class MapBuilderEditor : Editor
    {
        MapBuilder mapBuilder;

        Cell[,] cells;

        GameObject cellsHolderObj;
        GameObject roadsHolderObj;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            mapBuilder = target as MapBuilder;

            if (GUILayout.Button("New Map"))
            {
                BuildMap();
            }

            if (GUILayout.Button("Roads Only"))
            {
                RoadsOnly();
            }

            if (GUILayout.Button("Set Player Numbers"))
            {
                SetPlayerNumbers();
            }

            if (GUILayout.Button("Delete"))
            {
                DeleteCells();
                DeleteAnchors();
            }
        }

        void BuildMap()
        {

            DeleteCells();

            CreateCells();

            DeleteRoads();




            DeleteAnchors();

        }

        void SetAdjacencies()
        {
            foreach (var cellA in cells)
            {
                if (cellA == null)
                {
                    continue;
                }

                for (int i = 0; i < Cell.shapeDimension; i++)
                {
                    var coordinateB = cellA.axialCoordinate.AdjacentCoordinate((Direction)i);
                    Cell cellB;

                    try
                    {
                        cellB = cells[coordinateB.j, coordinateB.i];
                    }
                    catch
                    {
                        cellB = null;
                    }

                    Undo.RegisterCompleteObjectUndo(cellA, "Added Adjacent Cell");
                    cellA.adjacentCells[i] = cellB;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(cellA);
                }
            }
        }

        void BuildRoads()
        {
            roadsHolderObj = new GameObject("Roads");

            foreach (var cellA in cells)
            {
                if (cellA == null) continue;

                for (int i = 0; i < Map.numDirections; i++)
                {
                    Cell cellB = cellA.adjacentCells[i];
                    if (cellB == null || cellA.incidentRoads[i] != null) continue;

                    Road newRoad = BuildRoad(cellA, cellB);

                    Undo.RegisterCompleteObjectUndo(cellA, "Added Adjacent Road");
                    Undo.RegisterCompleteObjectUndo(cellB, "Added Adjacent Road");

                    cellA.incidentRoads[i] = newRoad;
                    cellB.incidentRoads[System.Array.IndexOf(cellB.adjacentCells, cellA)] = newRoad;

                    PrefabUtility.RecordPrefabInstancePropertyModifications(cellA);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(cellB);
                }
            }
        }

        Road BuildRoad(Cell a, Cell b)
        {
            Quaternion roadRotation = Quaternion.LookRotation(a.transform.position - b.transform.position, Vector3.up);
            Vector3 roadPosition = (a.transform.position + b.transform.position) * 0.5f;

            GameObject roadGameObject = PrefabUtility.InstantiatePrefab(mapBuilder.roadPrefab) as GameObject;

            Undo.RegisterCompleteObjectUndo(roadGameObject, "New Road");

            roadGameObject.transform.position = roadPosition;
            roadGameObject.transform.rotation = roadRotation;
            roadGameObject.layer = 6; // same as cells
            roadGameObject.transform.SetParent(roadsHolderObj.transform);

            var road = roadGameObject.GetComponent<Road>();
            road.SetCells(a, b);

            PrefabUtility.RecordPrefabInstancePropertyModifications(roadGameObject);

            return road;
        }

        void CreateCells()
        {
            cellsHolderObj = new GameObject("Cells");

            int dimension = mapBuilder.gridDepth * 2 + 1;
            cells = new Cell[dimension, dimension];

            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                {
                    //if (i + j < map.gridDepth || i + j > 3 * map.gridDepth)
                    //{
                    //    continue;
                    //}

                    Vector3 position = ComputePosition(j, i);

                    GameObject cellGO = Instantiate(mapBuilder.cellPrefab, position, Quaternion.identity);
                    cellGO.transform.SetParent(cellsHolderObj.transform);

                    var cell = cellGO.GetComponent<Cell>();

                    Undo.RegisterCompleteObjectUndo(cell, "Added Adjacent Cell");
                    cell.j = j;
                    cell.i = i;
                    PrefabUtility.RecordPrefabInstancePropertyModifications(cell);

                    cells[j, i] = cell;
                }
            }
        }

        void DeleteCells()
        {
            var cell = FindObjectOfType<Cell>();
            if (cell != null)
            {
                DestroyImmediate(cell.transform.parent.gameObject);
            }
        }

        Vector3 ComputePosition(int j, int i)
        {
            int q = i - mapBuilder.gridDepth;
            int r = j - mapBuilder.gridDepth;

            float cos = Mathf.Cos(Mathf.Deg2Rad * (360.0f / Cell.shapeDimension));
            float sin = Mathf.Sin(Mathf.Deg2Rad * (360.0f / Cell.shapeDimension));

            Vector3 qOffset = new Vector3(q * mapBuilder.size, 0, 0);
            Vector3 rOffset = new Vector3(r * mapBuilder.size * cos, 0, -r * mapBuilder.size * sin);

            return qOffset + rOffset;
        }
    }
}
