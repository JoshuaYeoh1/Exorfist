using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeal : MonoBehaviour
{
    Player player;
    HPManager hp;

    [Header("Casting")]
    public GameObject castingBarPrefab;
    public Transform castingBarTr;
    float castTime=1;
    
    [Header("Trails")]
    public GameObject castTrailVFXPrefab;
    public Transform[] castTrailTr;

    [Header("Cast")]
    float regenTime=3;
    float regenHp=3;
    public Material healMeshEffectMaterial;

    [Header("Cooldown")]
    public Image radialBar;
    float cooldown=30;
    float radialFill;

    void Awake()
    {
        player=GetComponent<Player>();
        hp=GetComponent<HPManager>();
    }

    void Update()
    {
        if(radialBar.IsActive()) radialBar.fillAmount = radialFill;
    }

    bool canCast=true;

    public void StartCast()
    {
        regenHp = UpgradeManager.Current.GetHealSpeed();

        if(canCast && player.canCast && hp.hp<hp.hpMax-regenHp)
        {
            canCast=false;

            castingRt=StartCoroutine(Casting());

            GameEventSystem.Current.OnAbilityCasting(gameObject, "Heal");
        }
        else
        {
            //AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICooldown, transform.position, false);
            // input buffer spams the shit outta this
        }
    }
    
    bool isCasting;

    AudioSource sfxCastingLoop;

    Coroutine castingRt;
    IEnumerator Casting()
    {
        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Casting);

        isCasting=true;

        ShowCastingBar();

        player.anim.CrossFade("casting", .25f, 2, 0);

        EnableCastTrails();

        castTime = UpgradeManager.Current.GetHealCastTime();

        sfxCastingLoop = AudioManager.Current.LoopSFX(gameObject, SFXManager.Current.sfxCastingLoop);

        yield return new WaitForSeconds(castTime);

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        isCasting=false;

        player.anim.CrossFade("heal", .25f, 2, 0);

        if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);

        AudioManager.Current.PlaySFX(SFXManager.Current.sfxCharge, transform.position);
    }

    public void Release()
    {
        DisableCastTrails();

        GameEventSystem.Current.OnAbilityCast(gameObject, "Heal");

        StartHeal();

        AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal1, transform.position);
        AudioManager.Current.PlaySFX(SFXManager.Current.sfxHeal2, transform.position);
    }

    public void StartHeal()
    {
        healingRt = StartCoroutine(Healing());

        if(coolingRt!=null) StopCoroutine(coolingRt);
        coolingRt = StartCoroutine(Cooling());

        ModelManager.Current.AddMaterial(player.playerModel, healMeshEffectMaterial);

        regenHp = UpgradeManager.Current.GetHealSpeed();

        hp.regenHp = regenHp;
    }

    Coroutine healingRt;
    IEnumerator Healing()
    {
        regenTime = UpgradeManager.Current.healDuration;

        yield return new WaitForSeconds(regenTime);
        StopHeal();
    }

    public void StopHeal()
    {
        if(healingRt!=null) StopCoroutine(healingRt);
        
        hp.regenHp = hp.defaultRegenHp;
        
        ModelManager.Current.RemoveMaterial(player.playerModel, healMeshEffectMaterial);

        GameEventSystem.Current.OnAbilityEnd(gameObject, "Heal");
    }

    public void Finish()
    {
        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        player.anim.CrossFade("cancel", .25f, 2, 0);
    }
    
    Coroutine coolingRt;
    IEnumerator Cooling()
    {
        cooldown = UpgradeManager.Current.GetHealCooldown();
        
        radialFill=1;
        TweenFill(0, cooldown);

        yield return new WaitForSeconds(cooldown);

        canCast=true;
    }
    
    int tweenFillLt=0;
    public void TweenFill(float to, float time=.01f)
    {
        LeanTween.cancel(tweenFillLt);
        tweenFillLt = LeanTween.value(radialFill, to, time)
            .setEaseInOutSine()
            .setOnUpdate( (float value)=>{radialFill=value;} )
            .id;
    }

    public void Cancel()
    {
        if(isCasting)
        {
            if(castingRt!=null) StopCoroutine(castingRt);

            canCast=true;

            isCasting=false;

            player.anim.CrossFade("cancel", .25f, 2, 0);

            Finish();

            HideCastingBar();

            DisableCastTrails();

            if(sfxCastingLoop) AudioManager.Current.StopLoop(sfxCastingLoop);
        }
    }

    GameObject bar;

    void ShowCastingBar()
    {
        bar = Instantiate(castingBarPrefab, castingBarTr.position, Quaternion.identity);

        bar.hideFlags = HideFlags.HideInHierarchy;
        bar.transform.parent = castingBarTr;

        castTime = UpgradeManager.Current.GetHealCastTime();

        FloatingBar fbar = bar.GetComponent<FloatingBar>();
        fbar.FillBar(0, 0);
        fbar.FillBar(1, castTime);

        HideCastingBar(castTime);
    }
    
    void HideCastingBar(float time=0)
    {
        if(bar) Destroy(bar, time);
    }

    List<GameObject> trails = new List<GameObject>();

    void EnableCastTrails()
    {
        for(int i=0; i<castTrailTr.Length; i++)
        {
            trails.Add( Instantiate(castTrailVFXPrefab, castTrailTr[i].position, Quaternion.identity) );
            trails[i].hideFlags = HideFlags.HideInHierarchy;
            trails[i].transform.parent = castTrailTr[i];
        }
    }

    void DisableCastTrails()
    {
        if(trails.Count==0) return;

        foreach(GameObject trail in trails)
        {
            if(trail) Destroy(trail);
        }
        trails.Clear();
    }

    public void ResetCooldown()
    {
        LeanTween.cancel(tweenFillLt);
        radialFill=0;

        if(coolingRt!=null) StopCoroutine(coolingRt);
        canCast=true;
    }

    void OnEnable()
    {
        GameEventSystem.Current.DeathEvent += OnDeath;
    }
    void OnDisable()
    {
        GameEventSystem.Current.DeathEvent -= OnDeath;
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(victim!=gameObject) return;

        Cancel();
        StopHeal();
        DisableCastTrails();
    }
}
