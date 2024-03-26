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
    
    void Start()
    {
        GameEventSystem.Current.StunEvent += OnStun;
        GameEventSystem.Current.RespawnEvent += OnRespawn;
    }
    void OnDestroy()
    {
        GameEventSystem.Current.StunEvent -= OnStun;
        GameEventSystem.Current.RespawnEvent -= OnRespawn;
    }

    public void OnStun(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;
        if(hurtInfo.stunTime<=0) return;
        if(!player.canStun) return;

        if(hurtInfo.stunTime>=currentStunTime)
        {
            currentStunTime=hurtInfo.stunTime;

            stunned=true;

            player.CancelActions();

            player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Stun);

            move.TweenInputClamp(hurtInfo.speedDebuffMult);

            StartCoroutine(RandStunAnim(hurtInfo.stunTime));

            CancelRecovering();
            RecoveringRt = StartCoroutine(Recovering(hurtInfo.stunTime));
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

            move.TweenInputClamp(1);

            player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
        }
    }

    void OnRespawn(GameObject zombo)
    {
        if(zombo!=gameObject) return;

        CancelRecovering();

        currentStunTime=0;

        stunned=false;

        move.TweenInputClamp(1, 0);
    }
}
