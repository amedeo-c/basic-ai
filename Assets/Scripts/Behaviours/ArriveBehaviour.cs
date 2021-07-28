using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class ArriveBehaviour : SteeringBehaviour
    {
        public Transform target;

        public float stopRadius;
        public float slowRadius;

        public override SteeringOutput GetOutput()
        {
            if (target == null)
            {
                return new SteeringOutput();
            }

            Vector3 direction = target.position - transform.position;
            float squaredDistance = direction.sqrMagnitude;

            if (squaredDistance < stopRadius * stopRadius)
            {
                return new SteeringOutput();
            }

            float speed = agent.maxLinearSpeed;

            if (squaredDistance < slowRadius * slowRadius)
            {
                speed *= (squaredDistance - stopRadius * stopRadius) / squaredDistance;
            }

            var output = new SteeringOutput(weight);

            output.linear = direction.normalized * speed;

            Debug.DrawLine(transform.position, transform.position + output.linear.normalized * 2, Color.green);

            return output;
        }
    }

}