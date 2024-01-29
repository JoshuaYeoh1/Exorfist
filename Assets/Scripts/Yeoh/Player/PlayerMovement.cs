using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3 dir;
    public FixedJoystick joystick;

    public float velocity, moveSpeed=10, acceleration=10, deceleration=10;
    public bool canMove=true;

    PlayerCombat combat;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
        combat=GetComponent<PlayerCombat>();
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(!combat.isAttacking)
        {
            if(joystick.Horizontal==0 && joystick.Vertical==0) // use keyboard wasd if joystick not touched
            {
                dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            }
            else // use joystick
            {
                dir = new Vector3(Mathf.Clamp(joystick.Horizontal, -1, 1), 0, Mathf.Clamp(joystick.Vertical, -1, 1));
            }
        }
        else
        {
            dir = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        velocity = rb.velocity.magnitude;

        if(canMove)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y=0;
            camRight.y=0;

            Move(dir.z, camForward.normalized);
            Move(dir.x, camRight.normalized);
        }
    }

    void Move(float magnitude, Vector3 direction)
    {
        float targetSpeed = magnitude * moveSpeed;

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = targetSpeed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    public void Push(float force, Vector3 direction, float stopTime)
    {
        canMove=false;
        CancelInvoke("EnableMove");
        Invoke("EnableMove", stopTime);

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        rb.AddForce(direction*force, ForceMode.Impulse);
    }

    public void EnableMove()
    {
        canMove=true;
    }
}
