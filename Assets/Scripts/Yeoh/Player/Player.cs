using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerStateMachine sm;
    [HideInInspector] public PlayerMovement move;
    [HideInInspector] public ClosestObjectFinder finder;
    [HideInInspector] public ManualTarget manual;
    [HideInInspector] public PlayerLook look;
    [HideInInspector] public InputBuffer buffer;
    [HideInInspector] public PlayerCombat combat;
    [HideInInspector] public PlayerAOE aoe;
    [HideInInspector] public PlayerLaser laser;
    [HideInInspector] public PlayerHeal heal;
    [HideInInspector] public HPManager hp;
    public PlayerGroundbox ground;
    
    public Animator anim;
    public GameObject playerModel;
    public List<Hurtbox> hurtboxes;
    public GameObject target;

    public bool isAlive=true, isGrounded, isPaused;
    public bool canMove, canTurn, canAttack, canBlock, canCast, canHurt, canStun, canTarget, canMeditate;

    Ragdoller ragdoll;
    [HideInInspector] public Transform respawnPoint;
    public CinemachineVirtualCamera faceCamera;

    public AudioSource voice;

    void Awake()
    {
        sm=GetComponent<PlayerStateMachine>();
        move=GetComponent<PlayerMovement>();
        finder=GetComponent<ClosestObjectFinder>();
        manual=GetComponent<ManualTarget>();
        look=GetComponent<PlayerLook>();
        buffer=GetComponent<InputBuffer>();
        combat=GetComponent<PlayerCombat>();
        aoe=GetComponent<PlayerAOE>();
        laser=GetComponent<PlayerLaser>();
        heal=GetComponent<PlayerHeal>();
        hp=GetComponent<HPManager>();
        ragdoll=GetComponent<Ragdoller>();

        GameEventSystem.Current.OnSpawn(gameObject, "Player");

        respawnPoint = new GameObject("Respawn Transform").transform;
    }

    void OnEnable()
    {
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.RespawnEvent += OnRespawn;
        GameEventSystem.Current.IntroCamStartEvent += OnIntroCamStart;
        GameEventSystem.Current.IntroCamEndEvent += OnIntroCamEnd;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.RespawnEvent -= OnRespawn;
        GameEventSystem.Current.IntroCamStartEvent -= OnIntroCamStart;
        GameEventSystem.Current.IntroCamEndEvent -= OnIntroCamEnd;
    }

    AudioSource sfxLowHpLoop;

    void Start()
    {
        sfxLowHpLoop = AudioManager.Current.LoopSFX(gameObject, SFXManager.Current.sfxUILowHealth, false, false);
        sfxLowHpLoop.volume=0;
    }

    void Update()
    {
        CheckTargetPriority();

        if(sfxLowHpLoop) sfxLowHpLoop.volume = 1-(hp.hp/hp.hpMax);
    }

    void CheckTargetPriority()
    {
        if(canTarget)
        {
            // prioritize the manual target
            if(manual.target)
            {
                if(target!=manual.target)
                {
                    target=manual.target;

                    GameEventSystem.Current.OnTarget(gameObject, target, true);
                }
            }
            else if(finder.target)
            {
                if(target!=finder.target)
                {
                    target=finder.target;

                    GameEventSystem.Current.OnTarget(gameObject, target, false);
                }
            }
            else target=null;
        }
        else
        {
            if(target) target=null;
        }
    }

    void FixedUpdate()
    {
        isGrounded = ground.isGrounded;
    }

    public void CancelActions()
    {
        combat.CancelAttack();
        aoe.Cancel();
        laser.Cancel();
        heal.Cancel();
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        CancelActions();

        RandDeathAnim();
        
        sm.TransitionToState(PlayerStateMachine.PlayerStates.Death);

        Respawn(3);

        CameraManager.Current.ChangeCamera(faceCamera);

        AudioManager.Current.PlayVoice(voice, SFXManager.Current.voicePlayerHurtHigh, false);

        UpgradeManager.Current.chi+=5;
    }

    void RandDeathAnim()
    {
        int i = Random.Range(1, 2);
        //anim.CrossFade("death"+i, .1f, 2, 0);
        anim.Play("death"+i, 2, 0);
    }

    public void Respawn(float wait=3)
    {
        StartCoroutine(Respawning(wait));
    }
    public IEnumerator Respawning(float wait)
    {
        yield return new WaitForSecondsRealtime(wait);

        ScenesManager.Current.PlayTransitionOut();
        yield return new WaitForSecondsRealtime(.1f);
        yield return new WaitForSecondsRealtime(ScenesManager.Current.transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        ScenesManager.Current.PlayTransitionIn();

        GameEventSystem.Current.OnRespawn(gameObject);
    }

    void OnRespawn(GameObject zombo)
    {
        if(zombo!=gameObject) return;

        hp.SetHPPercent(50);

        sm.TransitionToState(PlayerStateMachine.PlayerStates.Pause);

        ragdoll.ToggleRagdoll(false); // align to ragdoll first before teleporting
        
        transform.position = respawnPoint.position;
        transform.rotation = Quaternion.Euler(0, respawnPoint.rotation.y+180, 0);

        anim.Play("wake", 2, 0);

        ResetAbilityCooldowns();
    }

    public void ResetAbilityCooldowns()
    {
        aoe.ResetCooldown();
        laser.ResetCooldown();
        heal.ResetCooldown();
    }

    void OnIntroCamStart(GameObject cam)
    {
        sm.TransitionToState(PlayerStateMachine.PlayerStates.Pause);
    }

    void OnIntroCamEnd(GameObject cam)
    {
        sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        if(hp.hp>hp.hpMax*.66f)
        {
            AudioManager.Current.PlayVoice(voice, SFXManager.Current.voicePlayerHurtLow, false);
        }
        else if(hp.hp>hp.hpMax*.33f)
        {
            AudioManager.Current.PlayVoice(voice, SFXManager.Current.voicePlayerHurtMid, false);
        }
        else
        {
            AudioManager.Current.PlayVoice(voice, SFXManager.Current.voicePlayerHurtHigh, false);
        }
    }
}
