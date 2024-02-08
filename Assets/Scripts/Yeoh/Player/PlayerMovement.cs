using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    Rigidbody rb;

    public FixedJoystick joystick;
    [HideInInspector] public Vector3 dir;

    public float moveSpeed=10, acceleration=10, deceleration=10, velocity;
    [HideInInspector] public float defMoveSpeed;

    void Awake()
    {
        player=GetComponent<Player>();
        rb=GetComponent<Rigidbody>();

        defMoveSpeed = moveSpeed;
    }

    // void Update()
    // {
    //     CheckInput();
    // }

    public void CheckInput()
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

    public void NoInput()
    {
        dir = Vector3.zero;
    }

    void FixedUpdate()
    {
        if(player.canMove)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            camForward.y=0;
            camRight.y=0;

            Move(dir.z, camForward.normalized);
            Move(dir.x, camRight.normalized);
        }

        velocity = Mathf.Round(rb.velocity.magnitude*100)/100;
    }

    void Move(float magnitude, Vector3 direction)
    {
        float targetSpeed = magnitude * moveSpeed;

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = targetSpeed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    int tweenSpeedLt=0;
    public void TweenSpeed(float to, float time=.2f)
    {
        LeanTween.cancel(tweenSpeedLt);
        tweenSpeedLt = LeanTween.value(moveSpeed, to, time).setEaseInOutSine().setOnUpdate(UpdateTweenSpeed).id;
    }
    void UpdateTweenSpeed(float value)
    {
        moveSpeed = value;
    }

    public void Push(float force, Vector3 direction) //, float stopTime)
    {
        // if(disablingMoveRt!=null) StopCoroutine(disablingMoveRt);
        // disablingMoveRt = StartCoroutine(DisablingMove(stopTime));

        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        rb.AddForce(direction*force, ForceMode.Impulse);
    }

    // Coroutine disablingMoveRt;

    // IEnumerator DisablingMove(float time)
    // {
    //     player.canMove=false;
    //     yield return new WaitForSeconds(time);
    //     player.canMove=true;
    // }
}
