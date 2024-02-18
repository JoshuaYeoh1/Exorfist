using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [HideInInspector] public Player player;
    PlayerMovement move;
    PlayerCombat combat;
    [HideInInspector] public PlayerHurt hurt;
    [HideInInspector] public OffsetMeshColor color;
    PlayerStun stun;
    [HideInInspector] public FlashSpriteVFX flash;
    [HideInInspector] public ShockwaveVFX shock;
    public PlayerBlockMeter meter;
    InputBuffer buffer;
    PlayerCataclysmWave aoe;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.3f, blockKnockbackResistMult=.3f;
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
        shock=GetComponent<ShockwaveVFX>();
        buffer=GetComponent<InputBuffer>();
        aoe=GetComponent<PlayerCataclysmWave>();
    }

    void Update()
    {
        player.anim.SetBool("isBlocking", isBlocking);
    }

    public bool pressingBtn;

    [HideInInspector] public bool canBlock=true;

    public void Parry()
    {
        if(canBlock && !meter.IsEmpty() && player.canBlock)
        {
            canBlock=false;

            combat.CancelAttack();

            aoe.Cancel();

            stun.Recover();

            StartCoroutine(Parrying());

            RandParryAnim();

            move.TweenSpeed(move.defMoveSpeed*blockMoveSpeedMult);

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Parry);

            buffer.lastPressedBlock=-1;
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

    public void Block()
    {
        isBlocking=true;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Block);

        meter.CancelRegenCooling();
    }

    public void Unblock()
    {
        isBlocking=false;

        move.TweenSpeed(move.defMoveSpeed);

        if(!canBlock) StartCoroutine(BlockCoolingDown());

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        meter.RedoRegenCooling();
    }

    IEnumerator BlockCoolingDown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock=true;
    }

    public void CheckBlock(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(isParrying)
        {
            ParrySuccess(contactPoint);
        }
        else if(isBlocking)
        {
            meter.Hit(dmg, kbForce, contactPoint, speedDebuffMult, stunTime);
        }
        else
        {
            hurt.Hit(dmg, kbForce, contactPoint, speedDebuffMult, stunTime);
        }        
    }

    public void ParrySuccess(Vector3 contactPoint)
    {
        canBlock=true;

        meter.Refill(parryRefillPercent);

        flash.SpawnFlash(contactPoint, Color.yellow);

        color.FlashColor(.1f, -.5f, .5f, -.5f); // flash green

        Singleton.instance.SpawnPopUpText(player.popUpTextPos.position, "PARRY!", Color.green);

        Singleton.instance.CamShake();

        shock.SpawnShockwave(contactPoint, Color.green);

        //Singleton.instance.HitStop(); // fucks up your timing
    }
}
