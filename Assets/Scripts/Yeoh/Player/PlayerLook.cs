using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    Rigidbody rb;
    PlayerBlock block;

    public float turnSpeed=10;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        rb=GetComponent<Rigidbody>();
        block=GetComponent<PlayerBlock>();
    }

    public void CheckLook()
    {
        if(player.canTurn) CheckTurn();
    }

    public void CheckTurn()
    {
        if(block.blockedPoint!=Vector3.zero)
        {
            TurnTowards(GetDir(block.blockedPoint, transform.position), turnSpeed*10);
        }
        else if(player.target) // if there is a target in range
        {
            TurnTowards(GetDir(player.target.transform.position, transform.position), turnSpeed); // face at target
        }
        else if(move.dir.sqrMagnitude>0) // if joystick is moved
        {
            TurnTowards(rb.velocity.normalized, turnSpeed); // face move direction
        }
    }

    void TurnTowards(Vector3 direction, float turnSpeed)
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
