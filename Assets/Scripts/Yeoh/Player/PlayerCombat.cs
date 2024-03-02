using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    InputBuffer buffer;

    [Header("Light Attack")]
    public List<AttackSO> lightCombo;
    public int lightComboCounter=-1;

    [Header("Heavy Attack")]
    public List<AttackSO> heavyCombo;
    public int heavyComboCounter=-1;

    [Header("Combo Delay")]
    public float comboCooldown=.5f;

    bool interrupted;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        buffer=GetComponent<InputBuffer>();
    }

    bool canCombo=true, canAttack=true;

    public void Attack(string type="light")
    {
        if(canCombo && canAttack && player.canAttack) // wait for cooldown
        {
            if(type=="light" && lightComboCounter < lightCombo.Count-1)
            {
                lightComboCounter++;

                player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

                interrupted=false;

                canAttack=canFinish=false;                

                player.anim.CrossFade(lightCombo[lightComboCounter].animName, .25f, 2, 0); //anim.Play but smoother

                if(endingComboRt!=null) StopCoroutine(endingComboRt);

                buffer.lastPressedLightAttack=-1;
            }

            else if(type=="heavy" && heavyComboCounter < heavyCombo.Count-1)
            {
                heavyComboCounter++;

                player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

                interrupted=false;

                canAttack=canFinish=false;
                
                player.anim.CrossFade(heavyCombo[heavyComboCounter].animName, .25f, 2, 0);

                if(heavyComboCounter == heavyCombo.Count-1) canCombo=false;

                if(endingComboRt!=null) StopCoroutine(endingComboRt);

                buffer.lastPressedHeavyAttack=-1;
            }
        }
    }

    public void AttackRelease(string type)
    {
        if(!interrupted)
        {
            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Attack);

            AttackSO atkSO=null;

            if(type=="light") atkSO = lightCombo[lightComboCounter];
            if(type=="heavy") atkSO = heavyCombo[heavyComboCounter];

            if(atkSO)
            {
                move.Push(atkSO.dash, transform.forward);
                    
                ChooseHitbox(atkSO);
            }
        }
    }

    void ChooseHitbox(AttackSO atkSO)
    {
        int i = atkSO.hitboxIndex;

        // copy and replace scriptable object's values to hitbox's values
        player.hitboxes[i].damage = atkSO.damage;
        player.hitboxes[i].knockback = atkSO.knockback;
        player.hitboxes[i].hasSweepingEdge = atkSO.hasSweepingEdge;
        player.hitboxes[i].shake = atkSO.shake;
        player.hitboxes[i].hitstop = atkSO.hitstop;
        player.hitboxes[i].shockwave = atkSO.shockwave;

        // enable and disable hitbox rapidly
        player.hitboxes[i].BlinkHitbox(atkSO.hitboxActiveDuration);
    }

    public void AttackRecover()
    {
        canAttack=canFinish=true;

        if(endingComboRt!=null) StopCoroutine(endingComboRt);
        endingComboRt = StartCoroutine(EndingCombo(comboCooldown));
    }

    bool canFinish;

    public void AttackFinish()
    {
        if(canFinish)
        {
            canFinish=false;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

            player.anim.CrossFade("cancel", .25f, 2, 0);
        }
    }

    Coroutine endingComboRt;
    IEnumerator EndingCombo(float time)
    {
        yield return new WaitForSeconds(time); // reset combo after stopping a short while
        ResetCombo();
    }

    void ResetCombo()
    {
        lightComboCounter = heavyComboCounter = -1;

        canAttack=canCombo=true;
    }

    public void CancelAttack()
    {
        interrupted=true;

        player.anim.CrossFade("cancel", .1f, 2, 0); // cancel anim

        lightComboCounter = heavyComboCounter = -1;

        canAttack=canCombo=false;

        if(endingComboRt!=null) StopCoroutine(endingComboRt);
        endingComboRt = StartCoroutine(EndingCombo(comboCooldown));
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
