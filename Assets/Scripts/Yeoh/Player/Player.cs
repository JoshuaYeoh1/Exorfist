using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public PlayerMovement move;
    [HideInInspector] public ClosestObjectFinder finder;
<<<<<<< HEAD
=======
    [HideInInspector] public ManualTarget manual;
>>>>>>> main
    [HideInInspector] public PlayerLook look;
    [HideInInspector] public InputBuffer buffer;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerAOE aoe;
    [HideInInspector] public PlayerLaser laser;
<<<<<<< HEAD
=======
    [HideInInspector] public PlayerHeal heal;
>>>>>>> main
    
    public Animator anim;
    public Transform popUpTextPos;
    public List<PlayerHitbox> hitboxes;
    public GameObject target;

<<<<<<< HEAD
    public bool isGrounded;
    public bool isAlive=true, canMove, canAttack, canBlock, canCast, canHurt, canStun;
=======
    public bool isAlive=true, isGrounded, canMove, canTurn, canAttack, canBlock, canCast, canHurt, canStun;
>>>>>>> main

    void Awake()
    {
        stateMachine=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
<<<<<<< HEAD
=======
        manual=GetComponent<ManualTarget>();
>>>>>>> main
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
        combat=GetComponent<PlayerCombat>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
<<<<<<< HEAD
=======
        heal=GetComponent<PlayerHeal>();

        GameEventSystem.current.OnSpawn(gameObject);
    }

    void Update()
    {
        CheckTargetPriority();
    }

    void CheckTargetPriority()
    {
        if(manual.target) target=manual.target;
        else if(finder.target) target=finder.target;
        else target=null;
>>>>>>> main
    }

    public void CancelActions()
    {
        combat.CancelAttack();
        aoe.Cancel();
        laser.Cancel();
<<<<<<< HEAD
=======
        heal.Cancel();
>>>>>>> main
    }
}
