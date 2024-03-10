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
    ClosestObjectFinder finder;

    public GameObject sparksVFXPrefab;

    public float blockCooldown=.5f, parryWindowTime=.2f, blockMoveSpeedMult=.5f, blockKnockbackResistMult=.5f;
    public float parryRefillPercent=25;
    
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

            move.TweenMoveInputClamp(blockMoveSpeedMult);

            player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Parry);

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

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Block);

        meter.CancelRegenCooling();
    }

    public void Unblock()
    {
        isBlocking=false;

        move.TweenMoveInputClamp(1);

        if(!canBlock) StartCoroutine(BlockCoolingDown());

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        meter.RedoRegenCooling();
    }

    IEnumerator BlockCoolingDown()
    {
        yield return new WaitForSeconds(blockCooldown);
        canBlock=true;
    }

    public void CheckBlock(GameObject attacker, GameObject victim, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim!=gameObject) return;
        if(!player.isAlive) return;

        if(isParrying)
        {
            ParrySuccess(attacker, contactPoint);

            SetBlockedPoint(contactPoint);

            finder.ChangeInnerTarget(attacker);
        }
        else if(isBlocking)
        {
            meter.Hurt(attacker, dmg, kbForce, contactPoint);

            SetBlockedPoint(contactPoint);

            finder.ChangeInnerTarget(attacker);
        }
        else
        {
            hurt.Hurt(attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime);
        }   
    }

    public void ParrySuccess(GameObject attacker, Vector3 contactPoint)
    {
        canBlock=true;

        int i = Random.Range(1,3);
        player.anim.CrossFade("parry"+i, .1f, 3, 0);

        meter.Refill(parryRefillPercent);

        PlaySparkVFX(contactPoint, Color.green);

        hurt.DoIFraming(hurt.iframeTime, -.5f, .5f, -.5f); // flicker green

        GameEventSystem.Current.OnBlock(gameObject, attacker, contactPoint, true, false);



        // move to vfx manager later

        VFXManager.Current.SpawnPopUpText(ModelManager.Current.GetTopVertex(gameObject), "PARRY!", Color.green);

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

    public void PlaySparkVFX(Vector3 contactPoint, Color color)
    {
        flash.SpawnFlash(contactPoint, color);
        GameObject spark = Instantiate(sparksVFXPrefab, contactPoint, Quaternion.identity);
        spark.hideFlags = HideFlags.HideInHierarchy;
    }

    void Update() // testing
    {
        if(Input.GetKeyDown(KeyCode.Delete)) CheckBlock(null, gameObject, 10, 1, GetComponent<TopVertexFinder>().GetTopVertex(gameObject), .3f, 1);
    }
}
