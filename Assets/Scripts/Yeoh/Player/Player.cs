using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public PlayerMovement move;
    [HideInInspector] public ClosestObjectFinder finder;
    [HideInInspector] public PlayerLook look;
    [HideInInspector] public InputBuffer buffer;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerAOE aoe;
    [HideInInspector] public PlayerLaser laser;
    
    public Animator anim;
    public Transform popUpTextPos;
    public List<PlayerHitbox> hitboxes;

    public bool isGrounded;
    public bool isAlive=true, canMove, canAttack, canBlock, canCast, canHurt, canStun;

    void Awake()
    {
        stateMachine=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
        combat=GetComponent<PlayerCombat>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
    }

    public void CancelActions()
    {
        combat.CancelAttack();
        aoe.Cancel();
        laser.Cancel();
    }
}
