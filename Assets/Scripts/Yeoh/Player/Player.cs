using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine sm;
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
        sm=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        manual=GetComponent<ManualTarget>();
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
        combat=GetComponent<PlayerCombat>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
        heal=GetComponent<PlayerHeal>();

        GameEventSystem.Current.OnSpawn(gameObject, "Player");
    }

    void OnEnable()
    {
        GameEventSystem.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        GameEventSystem.Current.DeathEvent -= OnDeath;
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

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        CancelActions();
        heal.StopHeal();
        laser.StopLaser();

        RandDeathAnim();
        
        sm.TransitionToState(PlayerStateMachine.PlayerStates.Death);
    }

    void RandDeathAnim()
    {
        int i = Random.Range(1, 2);
        anim.CrossFade("death"+i, .1f, 2, 0);
    }
}
