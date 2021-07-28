using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Range(0, 100)]
        public int weight = 1;

        public int evaluationOrder = 0;

        protected Agent agent;

        protected virtual void Awake()
        {
            agent = GetComponent<Agent>();
        }

        public abstract SteeringOutput GetOutput();

        #region BASIC STEERING

        protected SteeringOutput Flee(Vector3 targetPosition, float criticalDistance = Mathf.Infinity)
        {
            Vector3 direction = targetPosition - agent.transform.position;

            if (direction.sqrMagnitude < criticalDistance * criticalDistance)
            {
                var output = new SteeringOutput(weight);
                output.linear = -(direction.normalized * agent.maxLinearSpeed);
                return output;
            }

            return new SteeringOutput();
        }

        protected SteeringOutput Seek(Vector3 targetPosition, float criticalDistance = 0f)
        {

            Vector3 direction = targetPosition - agent.transform.position;

            if (direction.sqrMagnitude > criticalDistance * criticalDistance)
            {
                var output = new SteeringOutput(weight);
                output.linear = (direction.normalized * agent.maxLinearSpeed);
                return output;
            }

            return new SteeringOutput();
        }

        #endregion

        protected void DebugOutput(SteeringOutput output, Color col)
        {
            Debug.DrawLine(transform.position + Vector3.up * 1.2f, transform.position + output.linear + Vector3.up * 1.2f, col);
        }
    }
}
