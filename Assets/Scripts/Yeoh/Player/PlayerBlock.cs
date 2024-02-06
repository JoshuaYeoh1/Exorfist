using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    PlayerCombat combat;
    PlayerHurt hurt;
    OffsetMeshColor color;
    PlayerStun stun;
    FlashSpriteVFX flash;

    public PlayerBlockMeter meter;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.3f, blockKnockbackResistMult=.3f;
    public float blockBreakSpeedDebuffMult=.5f, blockBreakPenaltyStunTime=1;
    public float parryRefillPercent=33;
    
    public bool isParrying, isBlocking;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        combat=GetComponent<PlayerCombat>();
        hurt=GetComponent<PlayerHurt>();
        color=GetComponent<OffsetMeshColor>();
        stun=GetComponent<PlayerStun>();
        flash=GetComponent<FlashSpriteVFX>();
    }

    void Update()
    {
        CheckInput();

        player.anim.SetBool("isBlocking", isBlocking);
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Q)) OnBtnDown();
        if(Input.GetKeyUp(KeyCode.Q)) OnBtnUp();
    }

    bool pressingBtn;

    public void OnBtnDown()
    {
        pressingBtn=true;

        Parry();
    }

    public void OnBtnUp()
    {
        pressingBtn=false;

        if(isBlocking) Unblock();
    }

    bool canBlock=true;

    void Parry()
    {
        if(player.canBlock && canBlock && meter.EnoughToBlock())
        {
            canBlock=false;

            combat.CancelAttack();

            stun.Recover();

            StartCoroutine(Parrying());

            RandParryAnim();

            move.moveSpeed = move.defMoveSpeed*blockMoveSpeedMult;

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Parry);
        }
    }

    void RandParryAnim()
    {
        int i = Random.Range(1, 3);
        player.anim.CrossFade("parry"+i, .1f, 3, 0);
    }

    IEnumerator Parrying()
    {
        isParrying=true;
        yield return new WaitForSeconds(parryWindowTime);
        isParrying=false;

        if(pressingBtn) Block();

        else Unblock();
    }

    void Block()
    {
        isBlocking=true;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Block);

        meter.CancelRegenCooling();
    }

    void Unblock()
    {
        isBlocking=false;

        move.moveSpeed = move.defMoveSpeed;

        if(!canBlock) StartCoroutine(BlockCoolingDown());

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        meter.RedoRegenCooling();
    }

    IEnumerator BlockCoolingDown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock=true;
    }

    public void ParrySuccess(Vector3 contactPoint)
    {
        Singleton.instance.SpawnPopUpText(player.popUpTextPos.position, "PARRY!", Color.green); //Debug.Log("Player Parry Success");

        canBlock=true;

        color.FlashColor(.1f, -.5f, .5f, -.5f); // flash green

        meter.Refill(parryRefillPercent);

        Singleton.instance.CamShake();

        Singleton.instance.HitStop();

        flash.SpawnFlash(contactPoint, Color.green);
    }

    public void BlockHit(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(!meter.iframe)
        {
            meter.DoIFraming();

            player.anim.CrossFade("block hit", .1f, 4, 0);

            canBlock=true;

            color.FlashColor(.1f, .5f, .5f, .5f); // flash white

            meter.Hit(dmg, kbForce, contactPoint, speedDebuffMult, stunTime);

            if(kbForce>0)
            {
                hurt.Knockback(kbForce*blockKnockbackResistMult, contactPoint);

                Singleton.instance.CamShake();
            }

            //Singleton.instance.playSFX(Singleton.instance.sfxSubwoofer, transform.position, false);
        }
    }

    public void BlockBreak(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        Singleton.instance.SpawnPopUpText(player.popUpTextPos.position, "bREAK!", Color.red); //Debug.Log("Player Block Broken");

        Unblock();
        
        hurt.Hit(dmg*.5f, kbForce*blockKnockbackResistMult, contactPoint, speedDebuffMult*blockBreakSpeedDebuffMult, blockBreakPenaltyStunTime);

        Singleton.instance.CamShake();

        Singleton.instance.HitStop();
        
        //feedback

        player.canStun=false; // dont overwrite the slow stun with a quick stun

        Invoke("ReEnableStun", blockBreakPenaltyStunTime);
    }
    void ReEnableStun()
    {
        player.canStun=true;
    }
}
