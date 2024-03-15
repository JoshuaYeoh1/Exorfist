using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLaser : MonoBehaviour
{
    Player player;
    ClosestObjectFinder finder;

    [Header("Casting")]
    public GameObject castingBarPrefab;
    public Transform castingBarTr;
    float castTime=1;
    
    [Header("Trails")]
    public GameObject castTrailVFXPrefab;
    public Transform[] castTrailTr;

    [Header("Cast")]
    public Transform firepointTr;
    public GameObject hitboxPrefab;
    float range=10;
    float dmg;
    float sustainTime=5;
    public float damageInterval=.2f;

    [Header("Cooldown")]
    public Image radialBar;
    float cooldown=45;
    float radialFill;

    void Awake()
    {
        player=GetComponent<Player>();
        finder=GetComponent<ClosestObjectFinder>();
    }

    void Update()
    {
        if(radialBar.IsActive()) radialBar.fillAmount = radialFill;
    }

    bool canCast=true;

    public void StartCast()
    {
        if(canCast && player.canCast)
        {
            canCast=false;

            castingRt=StartCoroutine(Casting());

            GameEventSystem.Current.OnAbilityCasting(gameObject, "Laser");
        }
    }

    bool isCasting;

    Coroutine castingRt;
    IEnumerator Casting()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Casting);

        isCasting=true;

        ShowCastingBar();

        player.anim.CrossFade("casting", .25f, 2, 0);

        EnableCastTrails();

        castTime = UpgradeManager.Current.GetLaserCastTime();

        yield return new WaitForSeconds(castTime);

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        isCasting=false;

        player.anim.CrossFade("laser start", .25f, 2, 0);
    }

    public void Release()
    {
        GameEventSystem.Current.OnAbilityCast(gameObject, "Laser");

        StartLaser();
    }

    GameObject laser;
    List<Collider> laserColls = new List<Collider>();
    List<Hurtbox> hurtboxes = new List<Hurtbox>();

    public void StartLaser()
    {
        sustainingRt = StartCoroutine(Sustaining());

        range = UpgradeManager.Current.GetLaserRange();

        finder.innerRange=range+.25f;
        finder.outerRange=range+.25f;

        SpawnLaser();
    }

    void SpawnLaser()
    {
        laser = Instantiate(hitboxPrefab, firepointTr.position, firepointTr.rotation);

        laser.transform.parent = firepointTr;

        range = UpgradeManager.Current.GetLaserRange()*.1f;

        laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, range);

        hurtboxes.AddRange(laser.GetComponentsInChildren<Hurtbox>());

        foreach(Hurtbox hurtbox in hurtboxes)
        {
            hurtbox.owner = gameObject;

            dmg = UpgradeManager.Current.GetLaserDmg();

            hurtbox.dmg = dmg;
            hurtbox.dmgBlock = dmg;
        }

        laserColls.AddRange(laser.GetComponentsInChildren<Collider>());

        flashingHitboxRt = StartCoroutine(FlashingHitbox());
    }

    Coroutine flashingHitboxRt;
    IEnumerator FlashingHitbox()
    {
        while(true)
        {
            VFXManager.Current.CamShake(damageInterval, 1);

            ToggleLaserHitbox(true);

            yield return new WaitForSeconds(damageInterval);

            ToggleLaserHitbox(false);
        }
    }

    void ToggleLaserHitbox(bool toggle)
    {
        if(laserColls.Count==0) return;

        foreach(Collider coll in laserColls)
        {
            coll.enabled=toggle;
        }
    }

    Coroutine sustainingRt;
    IEnumerator Sustaining()
    {
        sustainTime = UpgradeManager.Current.laserDuration;

        yield return new WaitForSeconds(sustainTime);
        StopLaser();
    }

    void DespawnLaser()
    {
        if(flashingHitboxRt!=null) StopCoroutine(flashingHitboxRt);

        ToggleLaserHitbox(false);

        laserColls.Clear();

        Destroy(laser);
    }

    public void StopLaser()
    {
        DespawnLaser();

        if(sustainingRt!=null) StopCoroutine(sustainingRt);

        player.anim.CrossFade("laser finish", .1f, 2, 0);

        finder.innerRange=finder.defInnerRange;
        finder.outerRange=finder.defOuterRange;

        DisableCastTrails();

        StartCoroutine(Cooling());

        GameEventSystem.Current.OnAbilityEnd(gameObject, "Laser");
    }

    public void Finish()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        player.anim.CrossFade("cancel", .25f, 2, 0);
    }

    IEnumerator Cooling()
    {
        cooldown = UpgradeManager.Current.GetLaserCooldown();

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
        }
    }

    GameObject bar;

    void ShowCastingBar()
    {
        bar = Instantiate(castingBarPrefab, castingBarTr.position, Quaternion.identity);
        bar.hideFlags = HideFlags.HideInHierarchy;
        bar.transform.parent = castingBarTr;

        castTime = UpgradeManager.Current.GetLaserCastTime();

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
        foreach(GameObject trail in trails)
        {
            if(trail) Destroy(trail);
        }
        trails.Clear();
    }
}
