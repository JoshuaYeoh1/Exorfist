using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    PlayerCombat combat;

    public bool stunned;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        combat=GetComponent<PlayerCombat>();
    }

    public void Stun(float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(player.canStun && stunTime>0)
        {
            stunned=true;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Stun);

            combat.CancelAttack();

            move.moveSpeed = move.defMoveSpeed*speedDebuffMult;

            StartCoroutine(RandStunAnim(stunTime));

            if(RecoveringRt!=null) StopCoroutine(RecoveringRt);
            RecoveringRt = StartCoroutine(Recovering(stunTime));
        }
    }

    IEnumerator RandStunAnim(float time)
    {
        player.anim.SetFloat("stunSpeed", 1);

        int i = Random.Range(1, 16);

        player.anim.Play("stun"+i, 4, 0);

        yield return null; // Wait for the next frame to ensure the animation state is updated

        float animLength = player.anim.GetCurrentAnimatorStateInfo(4).length;

        player.anim.SetFloat("stunSpeed", animLength/time);
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
