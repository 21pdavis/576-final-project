using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int SwarmIndex { get; set; }
    public float NoClumpingRadius { get; set; }
    public float LocalAreaRadius { get; set; }
    public float Speed { get; set; }
    public float SteeringSpeed { get; set; }

    public void SimulateMovement(List<BoidController> other, float time)
    {
        //default vars
        var steering = Vector3.zero;

        //apply steering
        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), SteeringSpeed * time);

        //move 
        transform.position += transform.TransformDirection(new Vector3(0, 0, Speed)) * time;

        //seperation 
        Vector3 seperation = Vector3.zero;
        int sep_count = 0;

        foreach (BoidController boid in other)
        {
            if (boid != this)
            {
                float distance = Vector3.Distance(transform.position, boid.transform.position);
                if (distance < NoClumpingRadius)
                {
                    seperation += (transform.position - boid.transform.position).normalized / distance;
                    sep_count++;
                }
            }
        }

        if (sep_count > 0)
        {
            seperation /= sep_count;
        }

        seperation = -seperation.normalized * Speed;
        steering = seperation;

        Vector3 alignment = Vector3.zero;   
        int align_count = 0;

        foreach (BoidController boid in other)
        {
            if (boid != this)
            {
                float distance = Vector3.Distance(transform.position, boid.transform.position);
                if (distance < LocalAreaRadius)
                {
                    alignment += boid.transform.forward;
                    align_count++;
                }
            }

        }

        steering += alignment.normalized * Speed;


        //cohesion
        Vector3 cohesion = Vector3.zero;
        int cohesion_count = 0;

        foreach (BoidController boid in other)
        {
            if (boid != this)
            {
                float distance = Vector3.Distance(transform.position, boid.transform.position);
                if (distance < LocalAreaRadius)
                {
                    cohesion += boid.transform.position;
                    cohesion_count++;
                }
            }
        }

        cohesion -= transform.position;
        steering += cohesion.normalized * Speed;
        
    }
}
