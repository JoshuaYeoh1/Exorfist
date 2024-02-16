using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;
    PlayerMovement move;

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
    }

    public void OnBtnDown(string type="light")
    {
        if(player.canAttack) Attack(type);
    }

    bool canCombo=true, canAttack=true;

    void Attack(string type)
    {
        if(canCombo && canAttack) // wait for cooldown
        {
            if(type=="light" && lightComboCounter < lightCombo.Count-1)
            {
                lightComboCounter++;

                player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

                interrupted=false;

                canAttack=canFinish=false;                

                player.anim.CrossFade(lightCombo[lightComboCounter].animName, .25f, 2, 0); //anim.Play but smoother

                if(endingComboRt!=null) StopCoroutine(endingComboRt);
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
            }
        }
    }

    public void AttackRelease(string type)
    {
        if(!interrupted)
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

        // copy and replace scriptable object's values to hitbox's values
        player.hitboxes[i].damage = aSO.damage;
        player.hitboxes[i].knockback = aSO.knockback;
        player.hitboxes[i].hasSweepingEdge = aSO.hasSweepingEdge;

        // enable and disable hitbox rapidly
        if(blinkingHitboxRt!=null) StopCoroutine(blinkingHitboxRt);
        StartCoroutine(BlinkingHitbox(i)); 
    }

    Coroutine blinkingHitboxRt;
    IEnumerator BlinkingHitbox(int i)
    {
        foreach(PlayerHitbox hitbox in player.hitboxes) // make sure all hitboxes are disabled
        {
            hitbox.ToggleActive(false);
        }

        player.hitboxes[i].ToggleActive(true);
        yield return new WaitForSeconds(.2f);
        player.hitboxes[i].ToggleActive(false);
    }

    public void AttackRecover()
    {
        canAttack=canFinish=true;

        if(endingComboRt!=null) StopCoroutine(endingComboRt);
        endingComboRt = StartCoroutine(EndingCombo(comboCooldown));
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

        ResetCombo();
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
