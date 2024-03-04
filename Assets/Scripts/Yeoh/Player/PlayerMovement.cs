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

    [HideInInspector] public float speedClamp=1; // for modifiying move speed

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
            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        else // use joystick
        {
            dir = new Vector3(Mathf.Clamp(joystick.Horizontal, -1, 1), 0, Mathf.Clamp(joystick.Vertical, -1, 1));
        }
    }

    void NoInput()
    {
        dir = Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y=0;
        camRight.y=0;

        dir = new Vector3
        (
            Mathf.Clamp(dir.x*1.5f, -speedClamp, speedClamp),
            0,
            Mathf.Clamp(dir.z*1.5f, -speedClamp, speedClamp)
        );

        Move(dir.z, moveSpeed, camForward.normalized);
        Move(dir.x, moveSpeed, camRight.normalized);

        velocity = Round(rb.velocity.magnitude, 2);
    }

    void Move(float mult, float magnitude, Vector3 direction)
    {
        float _mult = Mathf.Clamp(mult, -1, 1);
        
        float targetSpeed = _mult * magnitude;

        float accelRate = Mathf.Abs(targetSpeed)>0 ? acceleration:deceleration; // use decelerate value if no input, and vice versa
    
        float speedDif = targetSpeed - Vector3.Dot(direction, rb.velocity); // difference between current and target speed

        float movement = Mathf.Abs(speedDif) * accelRate * Mathf.Sign(speedDif); // slow down or speed up depending on speed difference

        rb.AddForce(direction * movement);
    }

    float Round(float num, int decimalPlaces)
    {
        return Mathf.Round(num * (10*decimalPlaces) ) / (10*decimalPlaces);
    }

    int tweenSpeedClampLt=0;
    public void TweenSpeedClamp(float to, float time=.25f)
    {
        LeanTween.cancel(tweenSpeedClampLt);
        tweenSpeedClampLt = LeanTween.value(speedClamp, to, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{speedClamp=value;} )
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
