using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{
    Rigidbody rb;

    [HideInInspector] public Vector3 dir;

    public float moveSpeed=10, acceleration=10, deceleration=10;
    [HideInInspector] public float defMoveSpeed;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();

        defMoveSpeed = moveSpeed;
    }

    void FixedUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y=0;
        camRight.y=0;

        Move(dir.z, camForward.normalized);
        Move(dir.x, camRight.normalized);
    }

    void Move(float magnitude, Vector3 direction)
    {
        float targetSpeed = magnitude * moveSpeed;

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = targetSpeed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    public void Push(float force, Vector3 direction)
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        rb.AddForce(direction*force, ForceMode.Impulse);
    }
}
