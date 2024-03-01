using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [HideInInspector] public Player player;
    PlayerMovement move;
    PlayerHurt hurt;
    [HideInInspector] public OffsetMeshColor color;
    PlayerStun stun;
    [HideInInspector] public FlashSpriteVFX flash;
    [HideInInspector] public ShockwaveVFX shock;
    public PlayerBlockMeter meter;
    InputBuffer buffer;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.3f, blockKnockbackResistMult=.3f;
    public float parryRefillPercent=33;
    
    public bool isParrying, isBlocking;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        hurt=GetComponent<PlayerHurt>();
        color=GetComponent<OffsetMeshColor>();
        stun=GetComponent<PlayerStun>();
        flash=GetComponent<FlashSpriteVFX>();
        shock=GetComponent<ShockwaveVFX>();
        buffer=GetComponent<InputBuffer>();
    }

    public bool pressingBtn;

    [HideInInspector] public bool canBlock=true;

    public void Parry()
    {
        if(canBlock && !meter.IsEmpty() && player.canBlock)
        {
            canBlock=false;

            player.CancelActions();

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
        Singleton.instance.CamShake();

        if(isParrying)
        {
            ParrySuccess(contactPoint);

            SetBlockedPoint(contactPoint);
        }
        else if(isBlocking)
        {
            meter.Hit(dmg, kbForce, contactPoint);

            SetBlockedPoint(contactPoint);
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

        flash.SpawnFlash(contactPoint, Color.green);

        hurt.DoIFraming(hurt.iframeTime, -.5f, .5f, -.5f); // flicker green

        Singleton.instance.SpawnPopUpText(player.popUpTextPos.position, "PARRY!", Color.green);

        shock.SpawnShockwave(contactPoint, Color.green);

        //Singleton.instance.HitStop(); // fucks up your timing
    }

    [HideInInspector] public Vector3 blockedPoint;

    void SetBlockedPoint(Vector3 point)
    {
        if(settingBlockedPointRt!=null) StopCoroutine(settingBlockedPointRt);
        settingBlockedPointRt=StartCoroutine(SettingBlockedPoint(point));
    }
    
    Coroutine settingBlockedPointRt;
    IEnumerator SettingBlockedPoint(Vector3 point)
    {
        blockedPoint = point;
        yield return new WaitForSeconds(.25f);
        blockedPoint = Vector3.zero;
    }

    void Update() // testing
    {
        if(Input.GetKeyDown(KeyCode.Delete)) CheckBlock(1, 1, transform.position, .3f, 1);
    }
}
