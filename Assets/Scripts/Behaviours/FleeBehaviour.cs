using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SteeringBehaviours
{
    public class FleeBehaviour : SteeringBehaviour
    {
        public Transform target;

        public override SteeringOutput GetOutput()
        {
            if (target != null)
            {
                return Flee(target.position);
            }
            else
            {
                return new SteeringOutput();
            }
        }
    }
}
