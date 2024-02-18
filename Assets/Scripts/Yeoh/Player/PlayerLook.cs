using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    ClosestObjectFinder finder;
    Rigidbody rb;

    public float turnSpeed=10;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        rb=GetComponent<Rigidbody>();
    }

    public void CheckLook()
    {
        if(finder.target) // if there is a target in range
        {
            FaceTowards(GetDir(finder.target.transform.position, transform.position)); // face at target
        }
        else if(move.dir.sqrMagnitude>0) // if joystick is moved
        {
            FaceTowards(rb.velocity.normalized); // face move direction
        }
    }

    void FaceTowards(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        lookRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y, 0f); // only rotate on the Y axis

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*turnSpeed); // smoothly face the direction
    }

    public Vector3 GetDir(Vector3 targetPos, Vector3 selfPos)
    {
        return (targetPos-selfPos).normalized;
    }
}
