using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    public Animator anim;

    [Header("Light Attack")]
    public List<AttackSO> lightCombo;
    public int lightComboCounter=-1;
    public float lightAttackCooldown=.5f;

    [Header("Heavy Attack")]
    public List<AttackSO> heavyCombo;
    public int heavyComboCounter=-1;
    public float heavyAttackCooldown=.8f;

    [Header("Combo Delay")]
    public float comboCooldown=.5f;
    public float resetComboAfter=.5f;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(player.canAttack)
        {
            if(Input.GetKeyDown(KeyCode.Space)) Attack("light");

            else if(Input.GetKeyDown(KeyCode.LeftAlt)) Attack("heavy");
        }
    }

    public void CheckBtn(string type="light")
    {
        if(player.canAttack)
        {
            Attack(type);
        }
    }

    bool canCombo=true, canAttack=true;

    void Attack(string type)
    {
        if(canCombo && canAttack) // wait for cooldown
        {
            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

            if(type=="light" && lightComboCounter < lightCombo.Count-1)
            {
                StartCoroutine(AttackCoolingDown(lightAttackCooldown));

                lightComboCounter++;

                anim.CrossFade(lightCombo[lightComboCounter].animName, .25f, 2, 0); //anim.Play but smoother

                EndComboAfter(lightAttackCooldown + resetComboAfter);
            }

            else if(type=="heavy" && heavyComboCounter < heavyCombo.Count-1)
            {
                StartCoroutine(AttackCoolingDown(heavyAttackCooldown));

                heavyComboCounter++;
                
                anim.CrossFade(heavyCombo[heavyComboCounter].animName, .25f, 2, 0);

                EndComboAfter(heavyAttackCooldown + resetComboAfter);

                if(heavyComboCounter == heavyCombo.Count-1)
                {
                    canCombo=false;

                    EndComboAfter(heavyAttackCooldown + resetComboAfter*2);
                }
            }
        }
    }

    IEnumerator AttackCoolingDown(float time)
    {
        canAttack=false;
        yield return new WaitForSeconds(time);
        canAttack=true;
    }

    public void AnimRelease(string type)
    {
        if(player.canAttack)
        {
            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Attack);

            AttackSO aSO=null;

            if(type=="light") aSO = lightCombo[lightComboCounter];
            if(type=="heavy") aSO = heavyCombo[heavyComboCounter];

            if(aSO)
            {
                move.Push(aSO.dash, transform.forward);
                    
                ChooseHitbox(aSO);
            }
        }
    }

    void ChooseHitbox(AttackSO aSO)
    {
        int i = aSO.hitboxIndex;

        player.hitboxes[i].damage = aSO.damage; // replace hitbox's damage value

        player.hitboxes[i].knockback = aSO.knockback; // replace hitbox's knockback value

        player.hitboxes[i].hasSweepingEdge = aSO.hasSweepingEdge; // if can swipe through enemies

        if(blinkingHitboxRt!=null) StopCoroutine(blinkingHitboxRt); // make sure to cancel before starting coroutine again
        StartCoroutine(BlinkingHitbox(i)); // enable and disable hitbox rapidly
    }

    Coroutine blinkingHitboxRt;

    IEnumerator BlinkingHitbox(int i)
    {
        foreach(PlayerWeapon hitbox in player.hitboxes) // make sure all hitboxes are disabled
        {
            hitbox.ToggleActive(false);
        }

        player.hitboxes[i].ToggleActive(true);
        yield return new WaitForSeconds(.2f);
        player.hitboxes[i].ToggleActive(false);
    }

    void EndComboAfter(float time)
    {
        if(endingComboRt!=null) StopCoroutine(endingComboRt);
        endingComboRt = StartCoroutine(EndingCombo(time));
    }
    Coroutine endingComboRt;
    IEnumerator EndingCombo(float time)
    {
        yield return new WaitForSeconds(time); // reset combo after stopping a short while
        EndCombo();
    }

    void EndCombo()
    {
        StartCoroutine(ComboCoolingDown(comboCooldown));

        lightComboCounter = heavyComboCounter = -1;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
    }

    IEnumerator ComboCoolingDown(float time)
    {
        canCombo=false;
        yield return new WaitForSeconds(time);
        canCombo=true;
    }

    // void CheckExitAttack() // INCONSISTENT
    // { 
    //     if(anim.GetCurrentAnimatorStateInfo(2).normalizedTime>=.7f && !anim.IsInTransition(2)) // after animation is certain % done and not transitioning
    //     {
    //         if(anim.GetCurrentAnimatorStateInfo(2).IsTag("Attack"))
    //         {
    //             Invoke("EndCombo", resetComboAfter); // reset combo after stopping a short while
    //         }
    //     }
    // }
}
