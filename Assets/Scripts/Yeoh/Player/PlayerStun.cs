using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    Player player;
    PlayerMovement move;

    public bool stunned;
    float currentStunTime;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    public void Stun(float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(stunTime>0 && stunTime>=currentStunTime && player.canStun)
        {
            currentStunTime=stunTime;

            stunned=true;

            player.CancelActions();

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Stun);

            move.TweenMoveInputClamp(speedDebuffMult);

            StartCoroutine(RandStunAnim(stunTime));

            CancelRecovering();
            RecoveringRt = StartCoroutine(Recovering(stunTime));
        }
    }

    IEnumerator RandStunAnim(float time)
    {
        player.anim.SetFloat("stunSpeed", 1);

        int i = Random.Range(1, 16);
        player.anim.Play("stun"+i, 3, 0);

        yield return null; // Wait a frame to ensure the animation state is updated

        float animLength = player.anim.GetCurrentAnimatorStateInfo(3).length;

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

            player.anim.CrossFade("cancel", .25f, 3, 0);

            CancelRecovering();

            move.TweenMoveInputClamp(1);

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }
}
