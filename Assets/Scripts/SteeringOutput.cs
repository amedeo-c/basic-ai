using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
    public class SteeringOutput
    {
        public Vector3 linear;
        public int weight;

        public SteeringOutput(int weight = 0)
        {
            linear = Vector3.zero;
            this.weight = weight;
        }

        public void Normalize()
        {
            if (weight == 0)
            {
                linear = Vector3.zero;
            }
            else
            {
                linear /= weight;
                weight = 1;
            }
        }


        public override string ToString()
        {
            return $"linear: {linear}\nweight: {weight}";
        }

        public static SteeringOutput operator +(SteeringOutput a, SteeringOutput b)
        {
            SteeringOutput output = new SteeringOutput();
            output.linear = (a.linear * a.weight) + (b.linear * b.weight);
            output.weight = a.weight + b.weight;
            return output;
        }

        public static SteeringOutput operator *(SteeringOutput a, float b)
        {
            SteeringOutput output = new SteeringOutput();
            output.linear = a.linear * b;
            output.weight = a.weight;
            return output;
        }

        public static SteeringOutput operator /(SteeringOutput a, float b)
        {
            SteeringOutput output = new SteeringOutput();
            output.linear = a.linear / b;
            output.weight = a.weight;
            return output;
        }
    }
}