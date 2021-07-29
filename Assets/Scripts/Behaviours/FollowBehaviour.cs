using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class FollowBehaviour : SteeringBehaviour
    {
        public Agent targetAgent;

        public float minDistance;
        public float maxDistance;
        public float boostFactor = 1.3f;

        public bool faceTarget;

        public override SteeringOutput GetOutput()
        {
            if (targetAgent == null)
            {
                return new SteeringOutput();
            }

            if(faceTarget) transform.LookAt(targetAgent.transform);

            Vector3 dir = targetAgent.transform.position - transform.position;

            float squareDist = dir.sqrMagnitude;
            if (squareDist < minDistance * minDistance)
            {
                return new SteeringOutput();
            }
            else
            {
                var output = new SteeringOutput(weight);

                if (squareDist > maxDistance * maxDistance)
                {
                    output.linear = dir.normalized * (targetAgent.currentVelocity.magnitude) * boostFactor;
                }
                else
                {
                    output.linear = dir.normalized * (targetAgent.currentVelocity.magnitude);
                }

                return output;
            }
        }
    }
}