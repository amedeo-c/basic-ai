using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class AvoidBehaviour : SteeringBehaviour
    {
        public float detectionRadius;

        public LayerMask detectionMask;

        public float maxAngle;
        public float oppositionAngle;

        public bool rightTendency;

        protected float obstacleAngle;
        protected Vector3 obstaclePoint;
        protected float obstacleDistance;

        public override SteeringOutput GetOutput()
        {
            var colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

            FindClosestObstacle(colliders);

            if (obstacleDistance > 10000f)
            {
                return new SteeringOutput();
            }
            else
            {
                Debug.DrawLine(transform.position, obstaclePoint, Color.yellow);

                Vector3 obstacleDir = obstaclePoint - transform.position;

                int sign = Vector3.SignedAngle(obstacleDir, agent.currentVelocity, Vector3.up) > 0f ? 1 : -1;

                float intensity = 1f - (Mathf.Abs(obstacleAngle) / maxAngle);

                Vector3 direction = Quaternion.AngleAxis(oppositionAngle, sign * Vector3.up) * obstacleDir.normalized;

                var output = new SteeringOutput(weight);

                output.linear = direction * (intensity * agent.maxLinearSpeed);

                DebugOutput(output, Color.yellow);

                return output;

            }
        }

        protected virtual void FindClosestObstacle(Collider[] colliders)
        {
            obstaclePoint = Vector3.zero;
            obstacleAngle = 0f;
            obstacleDistance = Mathf.Infinity;

            for (int i = 0; i < colliders.Length; i++)
            {
                var point = colliders[i].ClosestPoint(transform.position);

                var pointDir = point - transform.position;

                float angle = Vector3.SignedAngle(pointDir, agent.currentOutput.linear, Vector3.up);

                if (Mathf.Abs(angle) > maxAngle)
                {
                    continue;
                }

                float squaredDistance = pointDir.sqrMagnitude;

                if (squaredDistance < obstacleDistance)
                {
                    obstacleDistance = squaredDistance;
                    obstaclePoint = point;
                    obstacleAngle = angle;
                }
            }
        }
    }

}