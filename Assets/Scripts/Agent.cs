using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace SteeringBehaviours
{
    public class Agent : MonoBehaviour
    {
        public bool faceDirection = true;

        public float maxLinearSpeed;

        public float velocityStopThreshold = 0.1f;
        public float accelerationTimeStep = 0.2f;
        public float decelerationTimeStep = 0.15f;

        public SteeringOutput currentOutput;

        public Vector3 currentVelocity;

        List<SteeringOutput> outputQueue = new List<SteeringOutput>();

        SteeringBehaviour[] behaviours;

        public void EnqueueOutput(SteeringOutput output)
        {
            outputQueue.Add(output);
        }

        private void Start()
        {
            behaviours = GetComponents<SteeringBehaviour>();
            behaviours = behaviours.OrderBy(b => b.evaluationOrder).ToArray();
        }

        private void Update()
        {
            SteeringOutput output = ComputeTotalOutput();
            UpdateDynamics(output);
        }

        public void UpdateDynamics(SteeringOutput output)
        {
            float maxLinearAcceleration = maxLinearSpeed / accelerationTimeStep;
            float maxLinearDeceleration = maxLinearSpeed / decelerationTimeStep;

            Vector3 targetVelocity = output.linear;
            if (targetVelocity.sqrMagnitude > maxLinearSpeed * maxLinearSpeed)
            {
                targetVelocity = targetVelocity.normalized * maxLinearSpeed;
            }


            Vector3 acceleration = (targetVelocity - currentVelocity) / Time.deltaTime;

            if (acceleration.sqrMagnitude > maxLinearAcceleration * maxLinearAcceleration)
            {
                bool positive = targetVelocity.sqrMagnitude > currentVelocity.sqrMagnitude;

                if (positive)
                {
                    acceleration = acceleration.normalized * maxLinearAcceleration;
                }
                else
                {
                    acceleration = acceleration.normalized * maxLinearDeceleration;
                }
            }

            Vector3 newVelocity = currentVelocity + acceleration * Time.deltaTime;
            currentVelocity = newVelocity;

            if (currentVelocity.sqrMagnitude > velocityStopThreshold * velocityStopThreshold)
            {
                transform.position += currentVelocity * Time.deltaTime;

                if (faceDirection)
                {
                    transform.LookAt(transform.position + currentVelocity);
                }
            }
        }

        SteeringOutput ComputeTotalOutput()
        {
            currentOutput = new SteeringOutput();

            Vector3 linear = Vector3.zero;

            foreach (var o in outputQueue)
            {
                if (o.weight > 0)
                {
                    linear += o.linear;
                }

                currentOutput += o;
            }

            foreach (var b in behaviours)
            {
                currentOutput += b.GetOutput();
            }

            outputQueue.Clear();

            currentOutput.linear.y = 0;

            return currentOutput;
        }
    }

}