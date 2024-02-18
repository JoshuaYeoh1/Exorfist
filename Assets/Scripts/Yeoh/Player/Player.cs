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
    
    public Animator anim;
    public Transform popUpTextPos;
    public List<PlayerHitbox> hitboxes;

    public bool isAlive=true, canAttack, canBlock, canCast, canHurt, canStun;

    void Awake()
    {
        stateMachine=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
    }
}
