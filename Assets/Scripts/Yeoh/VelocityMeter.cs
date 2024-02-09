using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMeter : MonoBehaviour
{
    public Vector3 velocity, direction;
    Vector3 prevPos;

    void FixedUpdate()
    {
        Vector3 displacement = transform.position - prevPos;
        velocity = displacement / Time.deltaTime;

        prevPos = transform.position;
    }

    void Update()
    {
        float forwardDot = Vector3.Dot(transform.forward, velocity);
        float upDot = Vector3.Dot(transform.up, velocity);
        float rightDot = Vector3.Dot(transform.right, velocity);

        direction = new Vector3(rightDot, upDot, forwardDot).normalized;
    }
}
