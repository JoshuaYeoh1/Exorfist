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

                player.sm.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

                interrupted=false;

                canAttack=canFinish=false;                

                player.anim.CrossFade(lightCombo[lightComboCounter].animName, .25f, 2, 0); //anim.Play but smoother

                if(endingComboRt!=null) StopCoroutine(endingComboRt);

                buffer.lastPressedLightAttack=-1;

                AudioManager.Current.PlayVoice(player.voice, SFXManager.Current.voicePlayerAttackLow, false);
            }

            else if(type=="heavy" && heavyComboCounter < heavyCombo.Count-1)
            {
                heavyComboCounter++;

                player.sm.TransitionToState(PlayerStateMachine.PlayerStates.WindUp);

                interrupted=false;

                canAttack=canFinish=false;
                
                player.anim.CrossFade(heavyCombo[heavyComboCounter].animName, .25f, 2, 0);

                if(heavyComboCounter == heavyCombo.Count-1) canCombo=false;

                if(endingComboRt!=null) StopCoroutine(endingComboRt);

                buffer.lastPressedHeavyAttack=-1;

                if(heavyComboCounter < (heavyCombo.Count-1)*.5f)
                {
                    AudioManager.Current.PlayVoice(player.voice, SFXManager.Current.voicePlayerAttackLow, false);
                }
                else
                {
                    AudioManager.Current.PlayVoice(player.voice, SFXManager.Current.voicePlayerAttackHigh, false);
                }
            }
        }
    }

    public void AttackRelease(string type)
    {
        if(!interrupted)
        {
            player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Attack);

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
        player.hurtboxes[i].attackName = atkSO.attackName;
        player.hurtboxes[i].dmg = atkSO.dmg;
        player.hurtboxes[i].dmgBlock = atkSO.dmgBlock;
        player.hurtboxes[i].kbForce = atkSO.kbForce;
        player.hurtboxes[i].speedDebuffMult = atkSO.speedDebuffMult;
        player.hurtboxes[i].stunTime = atkSO.stunTime;
        player.hurtboxes[i].hasSweepingEdge = atkSO.hasSweepingEdge;
        player.hurtboxes[i].doImpact = atkSO.doImpact;
        player.hurtboxes[i].doShake = atkSO.doShake;
        player.hurtboxes[i].doHitstop = atkSO.doHitstop;
        player.hurtboxes[i].doShockwave = atkSO.doShockwave;
        player.hurtboxes[i].unparryable = atkSO.unparryable;

        // enable and disable hitbox rapidly
        player.hurtboxes[i].BlinkHitbox(atkSO.hitboxActiveDuration);
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

            player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

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
