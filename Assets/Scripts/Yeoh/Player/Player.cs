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
    public PlayerGroundbox ground;
    
    public Animator anim;
    public GameObject playerModel;
    public List<Hurtbox> hurtboxes;
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

        GameEventSystem.Current.OnSpawn(gameObject);
    }

    void Update()
    {
        CheckTargetPriority();
    }

    void CheckTargetPriority()
    {
        if(canTarget)
        {
            // prioritize the manual target
            if(manual.target)
            {
                if(target!=manual.target)
                {
                    target=manual.target;

                    GameEventSystem.Current.OnTarget(gameObject, target, true);
                }
            }
            else if(finder.target)
            {
                if(target!=finder.target)
                {
                    target=finder.target;

                    GameEventSystem.Current.OnTarget(gameObject, target, false);
                }
            }
            else
            {
                target=null;
            }
        }
        else
        {
            if(target) target=null;
        }
    }

    void FixedUpdate()
    {
        isGrounded = ground.isGrounded;
    }

    public void CancelActions()
    {
        combat.CancelAttack();
        aoe.Cancel();
        laser.Cancel();
        heal.Cancel();
    }
}
