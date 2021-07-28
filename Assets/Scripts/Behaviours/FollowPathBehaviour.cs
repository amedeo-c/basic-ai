using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CellPathFinding;

namespace SteeringBehaviours
{
    public class FollowPathBehaviour : SteeringBehaviour
    {
        [Tooltip("Used to change target cell along path")]
        public float arrivalRadius;
        [Tooltip("Used to stop at last target cell")]
        public float stopRadius;
        [Tooltip("Negative values for left tendency")]
        public float rightTendency;

        Cell targetCell;
        Vector3 targetPoint;
        int nextCellIndex;
        MapPath path = null;

        public MapPath Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                nextCellIndex = 1;

                if (path != null)
                {
                    if (path.Source != targetCell)
                    {
                        targetCell = path.Source;
                        targetPoint = TargetPoint(targetCell);
                    }
                    Debug.Assert(targetCell != null);
                }
                else
                {
                    targetCell = null;
                }
            }
        }

        public override SteeringOutput GetOutput()
        {
            var output = new SteeringOutput(weight);

            if (targetCell == null)
            {
                return output;
            }

            Vector3 direction = targetPoint - transform.position;

            if (targetCell == Path.Destination && direction.sqrMagnitude < stopRadius * stopRadius) 
            {
                Path = null;
                return output;
            }

            if (direction.sqrMagnitude < arrivalRadius * arrivalRadius)
            {
                if (targetCell != Path.Destination)
                {
                    targetCell = Path.GetCell(nextCellIndex);
                    Debug.Assert(targetCell != null);
                    nextCellIndex++;
                    targetPoint = TargetPoint(targetCell);
                    direction = targetPoint - transform.position;
                }
            }

            output.linear = direction.normalized * agent.maxLinearSpeed;

            return output;
        }

        Vector3 TargetPoint(Cell targetCell)
        {
            Vector3 direction = Vector3.zero;
            if (targetCell != null && transform != null)
            {
                direction = targetCell.transform.position - transform.position;
            }
            Vector2 leftPerpendicularVector = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
            float tendency = rightTendency * 0.5f + Random.Range(0, rightTendency * 0.5f);
            Vector3 offset = new Vector3(leftPerpendicularVector.x, 0, leftPerpendicularVector.y).normalized * -tendency;

            return targetCell.transform.position + offset;
        }
    }
}
