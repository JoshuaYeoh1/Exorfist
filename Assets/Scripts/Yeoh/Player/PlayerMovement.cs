using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3 dir;
    public FixedJoystick joystick;

    public float moveSpeed=10, acceleration=10, deceleration=10, velPower=1;
    public bool canMove=true;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
    }

    void Update()
    {
        checkInputs();
    }

    void checkInputs()
    {
        if(joystick.Horizontal==0 && joystick.Vertical==0)
        {
            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            dir = new Vector3(Mathf.Clamp(joystick.Horizontal, -1, 1), 0, Mathf.Clamp(joystick.Vertical, -1, 1));
        }
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y=0;
            camRight.y=0;

            move(dir.z, camForward.normalized);
            move(dir.x, camRight.normalized);
        }
    }

    void move(float inputAxis, Vector3 moveAxis)
    {
        float targetSpeed = inputAxis*moveSpeed;
    
        float speedDif = targetSpeed - Vector3.Dot(moveAxis, rb.velocity);

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate, velPower)*Mathf.Sign(speedDif);

        rb.AddForce(moveAxis*movement);
    }
}
