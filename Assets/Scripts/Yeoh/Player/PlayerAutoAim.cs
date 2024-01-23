using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoAim : MonoBehaviour
{
    Player player;
    PlayerMovement move;

    public float turnSpeed=100;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        faceMoveDirection();
    }

    void faceMoveDirection()
    {
        if(move.dir.sqrMagnitude>0 && move.canMove && player.targetsList.Count<=0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move.rb.velocity.normalized);

            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime*turnSpeed);
        }
    }
}
