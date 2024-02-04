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

    void FixedUpdate()
    {
        if(player.canLook)
        {
            if(!finder.target) // if no targets in range
            {
                if(player.canMove && move.dir.sqrMagnitude>0 && rb.velocity.normalized!=Vector3.zero) // if joystick is moved
                {
                    FaceTowards(rb.velocity.normalized); // face move direction
                }
            }

            else FaceTowards(GetDir(finder.target.transform.position, transform.position)); // else face at target
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
