using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerMovement move;
    ClosestObjectFinder finder;

    public bool canLook=true;
    public float turnSpeed=10;

    void Awake()
    {
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
    }

    void FixedUpdate()
    {
        if(canLook)
        {
            if(!finder.target) // if no targets in range
            {
                if(move.canMove && move.dir.sqrMagnitude>0 && move.rb.velocity.normalized!=Vector3.zero) // if joystick is moved
                {
                    FaceTowards(move.rb.velocity.normalized); // face move direction
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

    Vector3 GetDir(Vector3 targetPos, Vector3 selfPos)
    {
        return (targetPos-selfPos).normalized;
    }
}
