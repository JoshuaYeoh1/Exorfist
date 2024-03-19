using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    [HideInInspector] public Player player;
    PlayerMovement move;
    HurtScript hurt;
    PlayerStun stun;
    public PlayerBlockMeter meter;
    InputBuffer buffer;
    ClosestObjectFinder finder;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.5f, blockKnockbackResistMult=.5f;
    public float parryRefillPercent=25;
    
    public bool isParrying, isBlocking;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
        hurt=GetComponent<HurtScript>();
        stun=GetComponent<PlayerStun>();
        buffer=GetComponent<InputBuffer>();
        finder=GetComponent<ClosestObjectFinder>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.HitEvent += CheckBlock;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HitEvent -= CheckBlock;
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

            move.TweenInputClamp(blockMoveSpeedMult);

            player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Parry);

            buffer.lastPressedBlock=-1;
        }
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

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Block);

        meter.CancelRegenCooling();
    }

    public void Unblock()
    {
        isBlocking=false;

        move.TweenInputClamp(1);

        if(!canBlock) StartCoroutine(BlockCoolingDown());

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        meter.RedoRegenCooling();
    }

    IEnumerator BlockCoolingDown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock=true;
    }

    public void CheckBlock(GameObject attacker, GameObject victim, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;
        if(!player.isAlive) return;
        if(!player.canHurt) return;

        if(isParrying)
        {
            if(!hurtInfo.unparryable)
            {   
                ParrySuccess(attacker, hurtInfo);
            }
            else
            {
                meter.Hurt(attacker, hurtInfo);
            }

            SetBlockedPoint(hurtInfo.contactPoint);
        }
        else if(isBlocking)
        {
            meter.Hurt(attacker, hurtInfo);

            SetBlockedPoint(hurtInfo.contactPoint);
        }
        else
        {
            hurt.Hurt(attacker, hurtInfo);
        }

        //if(attacker) finder.ChangeInnerTarget(attacker);
        finder.ChangeInnerTarget();
    }

    public void ParrySuccess(GameObject attacker, HurtInfo hurtInfo)
    {
        canBlock=true;

        int i = Random.Range(1,3);
        player.anim.CrossFade("parry"+i, .1f, 3, 0);

        meter.Refill(parryRefillPercent);

        hurt.DoIFraming(hurt.iframeTime, -.5f, .5f, -.5f); // flicker green

        GameEventSystem.Current.OnParry(gameObject, attacker, hurtInfo);

        AudioManager.Current.PlayVoice(player.voice, SFXManager.Current.voicePlayerAttackLow, false);
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
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            HurtInfo hurtInfo = new HurtInfo();

            hurtInfo.dmg=10;
            hurtInfo.dmgBlock=10;
            hurtInfo.kbForce=1;
            hurtInfo.contactPoint=ModelManager.Current.GetColliderTop(gameObject);
            hurtInfo.speedDebuffMult=.3f;
            hurtInfo.stunTime=1;

            CheckBlock(null, gameObject, hurtInfo);
        }
    }
}
