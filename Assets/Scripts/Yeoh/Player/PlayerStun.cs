using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    PlayerCombat combat;

    public bool stunned;
    float currentStunTime;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        combat=GetComponent<PlayerCombat>();
    }

    public void Stun(float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(player.canStun && stunTime>0 && stunTime>currentStunTime)
        {
            currentStunTime=stunTime;

            stunned=true;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Stun);

            combat.CancelAttack();

            move.TweenSpeed(move.defMoveSpeed*speedDebuffMult);

            StartCoroutine(RandStunAnim(stunTime));

            CancelRecovering();
            RecoveringRt = StartCoroutine(Recovering(stunTime));
        }
    }

    IEnumerator RandStunAnim(float time)
    {
        player.anim.SetFloat("stunSpeed", 1);

        int i = Random.Range(1, 16);

        player.anim.Play("stun"+i, 4, 0);

        yield return null; // Wait a frame to ensure the animation state is updated

        float animLength = player.anim.GetCurrentAnimatorStateInfo(4).length;

        player.anim.SetFloat("stunSpeed", animLength/time);
    }

    void CancelRecovering()
    {
        if(RecoveringRt!=null)
        {
            StopCoroutine(RecoveringRt);
            RecoveringRt=null;
        }
    }
    Coroutine RecoveringRt;
    IEnumerator Recovering(float time)
    {
        yield return new WaitForSeconds(time);
        Recover();
    }

    public void Recover()
    {
        if(stunned && player.isAlive)
        {
            currentStunTime=0;

            stunned=false;

            player.anim.CrossFade("cancel", .25f, 4, 0);

            CancelRecovering();

            move.TweenSpeed(move.defMoveSpeed);

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }
}
