using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceVelocityDirection : MonoBehaviour
{
    Rigidbody rb;
    public bool enable=true;
    public float turnSpeed=100;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(enable && rb.velocity.sqrMagnitude>.01f)
        {
            Quaternion velocityDirection = Quaternion.FromToRotation(transform.forward, rb.velocity.normalized)*transform.rotation;

            transform.rotation = Quaternion.Lerp(transform.rotation, velocityDirection, Time.deltaTime*turnSpeed);
        }
    }
}
