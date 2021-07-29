using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;

namespace CellPathFinding
{
    public class TravelCostTest : MonoBehaviour
    {
        public PathFinder pf;

        public PFAlgorithm algorithm;

        public Cell sourceCell;
        public Cell targetCell;

        [ContextMenu("Calculate Path")]
        public void CalculatePath()
        {
            CalculatePath(sourceCell, targetCell);
        }

        void CalculatePath(Cell sourceCell, Cell targetCell)
        {
            if(sourceCell == null || targetCell == null)
            {
                Debug.LogError("Invalid Input.");
                return;
            }

            bool ArrivalCriterion(Cell cell)
            {
                return cell == targetCell;
            }

            float CellCostFunction(Cell cell)
            {
                return 10f;
            }

            float EdgeCostFunction(Edge edge)
            {
                return 10f;
            }

            MapPath path;
            if(algorithm == PFAlgorithm.Dijkstra)
            {
                path = pf.DijkstraPF(sourceCell, ArrivalCriterion, CellCostFunction, EdgeCostFunction);
            }
            else
            {
                path = pf.AStarPF(sourceCell, targetCell, CellCostFunction, EdgeCostFunction);
            }

            Debug.LogError($"path length: {path.Length}");
            path.Draw(5f);
        }
    }

    public enum PFAlgorithm
    {
        Dijkstra,
        AStar
    }
}