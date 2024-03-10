using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;
    Rigidbody rb;

    public FixedJoystick joystick;
    [HideInInspector] public Vector3 input;
    public float sensitivityFactor = 1.5f;

    public float moveSpeed=10, acceleration=10, deceleration=10, velocity;

    [HideInInspector] public float defMoveSpeed;
    [HideInInspector] public float inputClamp=1; // for speed debuffs

    void Awake()
    {
        player=GetComponent<Player>();
        rb=GetComponent<Rigidbody>();

        defMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if(player.canMove) CheckInput();
        else NoInput();
    }

    void CheckInput()
    {
        if(joystick.Horizontal==0 && joystick.Vertical==0) // use keyboard wasd if joystick not touched
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * sensitivityFactor;
        }
        else // use joystick
        {
            input = new Vector3(joystick.Horizontal, 0, joystick.Vertical) * sensitivityFactor;
        }

        if(input.magnitude>1) input.Normalize(); // never go past 1

        if(input.magnitude>inputClamp) input = input.normalized * inputClamp; // never go past the speed clamp
    }

    void NoInput()
    {
        input = Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y=0;
        camRight.y=0;

        Move(input.z, moveSpeed, camForward.normalized);
        Move(input.x, moveSpeed, camRight.normalized);

        velocity = Round(rb.velocity.magnitude, 2);
    }

    void Move(float mult, float magnitude, Vector3 direction)
    {
        float targetSpeed = mult * magnitude;

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = targetSpeed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    float Round(float num, int decimalPlaces)
    {
        return Mathf.Round(num * (10*decimalPlaces) ) / (10*decimalPlaces);
    }

    int tweenInputClampLt=0;
    public void TweenInputClamp(float to, float time=.25f)
    {
        LeanTween.cancel(tweenInputClampLt);
        tweenInputClampLt = LeanTween.value(inputClamp, to, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{inputClamp=value;} )
            .id;
    }

    int tweenSpeedLt=0;
    public void TweenSpeed(float to, float time=.25f)
    {
        LeanTween.cancel(tweenSpeedLt);
        tweenSpeedLt = LeanTween.value(moveSpeed, to, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{moveSpeed=value;} )
            .id;
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
