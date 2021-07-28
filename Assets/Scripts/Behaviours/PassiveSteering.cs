using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class PassiveSteering : SteeringBehaviour
    {
        public float seekDistance = 0f;
        public float fleeDistance = Mathf.Infinity;

        public override SteeringOutput GetOutput()
        {
            return new SteeringOutput();
        }

        public void PassiveSeek(Vector3 targetPosition)
        {
            var output = Seek(targetPosition, seekDistance);
            DebugOutput(output, Color.green);
            agent.EnqueueOutput(output);
        }

        public void PassiveFlee(Vector3 targetPosition)
        {
            var output = Flee(targetPosition, fleeDistance);
            DebugOutput(output, Color.red);
            agent.EnqueueOutput(output);
        }
    }

}