using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public PlayerMovement move;
    [HideInInspector] public ClosestObjectFinder finder;
    [HideInInspector] public ManualTarget manual;
    [HideInInspector] public PlayerLook look;
    [HideInInspector] public InputBuffer buffer;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerAOE aoe;
    [HideInInspector] public PlayerLaser laser;
    [HideInInspector] public PlayerHeal heal;
    
    public Animator anim;
    public Transform popUpTextPos;
    public List<PlayerHitbox> hitboxes;
    public GameObject target;

    public bool isAlive=true, isGrounded, canMove, canTurn, canAttack, canBlock, canCast, canHurt, canStun, canTarget;

    void Awake()
    {
        stateMachine=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        manual=GetComponent<ManualTarget>();
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
        combat=GetComponent<PlayerCombat>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
        heal=GetComponent<PlayerHeal>();

        GameEventSystem.current.OnSpawn(gameObject);
    }

    void Update()
    {
        CheckTargetPriority();
    }

    void CheckTargetPriority()
    {
        if(canTarget)
        {
            if(manual.target) target=manual.target;
            else if(finder.target) target=finder.target;
            else target=null;
        }
        else
        {
            target=null;
        }
    }

    public void CancelActions()
    {
        combat.CancelAttack();
        aoe.Cancel();
        laser.Cancel();
        heal.Cancel();
    }
}
