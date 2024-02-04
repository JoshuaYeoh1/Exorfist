using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    public Animator anim;

    public bool stunned;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    void Update() // testing
    {
        if(Input.GetKeyDown(KeyCode.Delete)) Stun();
    }

    public void Stun(float time=.5f, float speedDebuffMult=.3f)
    {
        if(player.canStun)
        {
            stunned=true;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Stun);

            anim.CrossFade("cancel", .1f, 2, 0); // cancel attack

            move.moveSpeed = move.defMoveSpeed*speedDebuffMult;

            RandStunAnim(time);

            CancelRecovery();

            RecoveringRt = StartCoroutine(Recovering(time));
        }
    }

    void RandStunAnim(float time)
    {
        int i = Random.Range(1, 16);

        anim.CrossFade("stun"+i, .05f, 4, 0);

        float animLength = anim.GetCurrentAnimatorStateInfo(4).length;

        anim.SetFloat("stunSpeed", animLength/time);
    }

    void CancelRecovery()
    {
        if(RecoveringRt!=null)
        {
            StopCoroutine(RecoveringRt);
            RecoveringRt =null;
        }
    }

    Coroutine RecoveringRt;
    IEnumerator Recovering(float time)
    {
        yield return new WaitForSeconds(time);

        Recover();
    }

    void Recover()
    {
        if(stunned && player.isAlive)
        {
            stunned=false;

            move.moveSpeed = move.defMoveSpeed;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }
}
