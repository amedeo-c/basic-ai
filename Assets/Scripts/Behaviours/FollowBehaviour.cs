using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class FollowBehaviour : SteeringBehaviour
    {
        public float minDistance;
        public float maxDistance;
        public float boostFactor = 1.3f;

        public bool faceTarget;

        public Transform target;

        public Agent targetAgent;

        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                targetAgent = target.GetComponent<Agent>();

                Debug.Assert(targetAgent != null);
            }
        }

        public override SteeringOutput GetOutput()
        {
            if (target == null)
            {
                return new SteeringOutput();
            }

            if(faceTarget) transform.LookAt(target);

            Vector3 dir = target.position - transform.position;

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