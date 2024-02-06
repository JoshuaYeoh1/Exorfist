using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public float speed=5, magnitude=5;

    Rigidbody rb;
    Vector3 initialPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        float horizontalMovement = Mathf.PingPong(Time.time * speed, magnitude * 2) - magnitude;

        rb.velocity = new Vector3(horizontalMovement, 0f, 0f);
    }
}
