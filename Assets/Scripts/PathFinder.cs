using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Queues;
using System.Linq;

namespace CellPathFinding
{ 
    public class PathFinder : MonoBehaviour
    {
        public int maxIterations;

        List<Cell> cells = new List<Cell>();

        [ContextMenu("Find Cells")]
        public void FindCells()
        {
            cells = FindObjectsOfType<Cell>().ToList();
        }

        public void SetCells(IEnumerable<Cell> cells)
        {
            this.cells = cells.ToList();
        }

        public MapPath DijkstraPF(Cell sourceCell, 
                                  System.Func<Cell, bool> arrivalCriterion, 
                                  System.Func<Cell, float> cellCostFunction, 
                                  System.Func<Edge, float> edgeCostFunction,
                                  float maxAllowedCost = Mathf.Infinity)
        {
            var previousOf = new Dictionary<Cell, Cell>();

            var cellComputedCost = new Dictionary<Cell, float>();
            foreach (var cell in cells)
            {
                cellComputedCost[cell] = Mathf.Infinity;
            }

            int PriorityComparison(Cell a, Cell b)
            {
                if(cellComputedCost[a] < cellComputedCost[b])
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            var openSet = new DelegateQueue<Cell>(PriorityComparison);
            var closedSet = new List<Cell>();

            cellComputedCost[sourceCell] = 0f;
            openSet.Enqueue(sourceCell);

            Cell endCell = null;

            int numIterations = 0;

            while (!openSet.Empty())
            {
                var current = openSet.Dequeue();

                if (arrivalCriterion(current))
                {
                    endCell = current;
                    break;
                }

                closedSet.Add(current);

                for (int i = 0; i < Cell.shapeDimension; i++)
                {
                    numIterations++;
                    if (numIterations > maxIterations)
                    {
                        Debug.LogError("max iterations exceeded.");
                        return null;
                    }

                    var neighbor = current.GetNeighbor(i);
                    var road = current.GetEdge(i);

                    if (neighbor == null || road == null)
                    {
                        continue;
                    }

                    float additionalCost = Mathf.Clamp(cellCostFunction(neighbor) + edgeCostFunction(road), 1f, 1000000f);
                    float neighborCost = cellComputedCost[current] + additionalCost;
                    if (neighborCost < cellComputedCost[neighbor])
                    {
                        cellComputedCost[neighbor] = neighborCost;
                        previousOf[neighbor] = current;
                    }

                    if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor);
                    }
                }
            }

            if (endCell == null)
            {
                return null;
            }

            numIterations = 0;
            var currentCell = endCell;
            var pathCells = new List<Cell>();


            while (currentCell != sourceCell)
            {
                pathCells.Add(currentCell);
                currentCell = previousOf[currentCell];

                numIterations++;
                if (numIterations > maxIterations)
                {
                    Debug.LogError("failed path reversing.");
                    Debug.LogError(new MapPath(pathCells));
                    return null;
                }
            }
            pathCells.Add(sourceCell);

            float totCost = 0f;
            for (int i = 1; i < pathCells.Count - 1; i++)
            {
                totCost += cellCostFunction(pathCells[i]);
            }
            if (totCost > maxAllowedCost)
            {
                return null;
            }

            if (pathCells.Count >= 1)
            {
                pathCells.Reverse();
                return new MapPath(pathCells);
            }
            else
            {
                return null;
            }
        }

        public MapPath AStarPF(Cell sourceCell,
                               Cell targetCell,
                               System.Func<Cell, float> cellCostFunction,
                               System.Func<Edge, float> edgeCostFunction,
                               float maxAllowedCost = Mathf.Infinity)
        {
            var previousOf = new Dictionary<Cell, Cell>();

            var cellComputedCost = new Dictionary<Cell, float>();
            foreach (var cell in cells)
            {
                cellComputedCost[cell] = Mathf.Infinity;
            }
            var cellPriority = new Dictionary<Cell, float>();
            foreach (var cell in cells)
            {
                cellPriority[cell] = Mathf.Infinity;
            }

            int PriorityComparison(Cell a, Cell b)
            {
                if (cellPriority[a] < cellPriority[b])
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }

            float Heuristic(Cell cell)
            {
                return (cell.GetPosition() - targetCell.GetPosition()).sqrMagnitude;
            }

            var openSet = new DelegateQueue<Cell>(PriorityComparison);
            var closedSet = new List<Cell>();

            cellComputedCost[sourceCell] = 0f;
            cellPriority[sourceCell] = 0f;
            openSet.Enqueue(sourceCell);

            Cell endCell = null;

            int numIterations = 0;

            while (!openSet.Empty())
            {
                var current = openSet.Dequeue();

                if (current == targetCell)
                {
                    endCell = current;
                    break;
                }

                closedSet.Add(current);

                for (int i = 0; i < Cell.shapeDimension; i++)
                {
                    numIterations++;
                    if (numIterations > maxIterations)
                    {
                        Debug.LogError("max iterations exceeded.");
                        return null;
                    }

                    var neighbor = current.GetNeighbor(i);
                    var road = current.GetEdge(i);

                    if (neighbor == null || road == null)
                    {
                        continue;
                    }

                    float additionalCost = Mathf.Clamp(cellCostFunction(neighbor) + edgeCostFunction(road), 1f, 1000000f);
                    float neighborCost = cellComputedCost[current] + additionalCost;
                    if (neighborCost < cellComputedCost[neighbor])
                    {
                        cellComputedCost[neighbor] = neighborCost;
                        cellPriority[neighbor] = neighborCost + Heuristic(neighbor);
                        previousOf[neighbor] = current;
                    }

                    if (!openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor);
                    }
                }
            }

            if (endCell == null)
            {
                return null;
            }

            numIterations = 0;
            var currentCell = endCell;
            var pathCells = new List<Cell>();


            while (currentCell != sourceCell)
            {
                pathCells.Add(currentCell);
                currentCell = previousOf[currentCell];

                numIterations++;
                if (numIterations > maxIterations)
                {
                    Debug.LogError("failed path reversing.");
                    Debug.LogError(new MapPath(pathCells));
                    return null;
                }
            }
            pathCells.Add(sourceCell);

            float totCost = 0f;
            for (int i = 1; i < pathCells.Count - 1; i++)
            {
                totCost += cellCostFunction(pathCells[i]);
            }
            if (totCost > maxAllowedCost)
            {
                return null;
            }

            if (pathCells.Count >= 1)
            {
                pathCells.Reverse();
                return new MapPath(pathCells);
            }
            else
            {
                return null;
            }
        }
    }

}