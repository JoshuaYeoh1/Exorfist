using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAOE : MonoBehaviour
{
    Player player;

    [Header("Casting")]
    public GameObject castingBarPrefab;
    public Transform castingBarTr;
    float castTime=1;
    
    [Header("Trails")]
    public GameObject castTrailVFXPrefab;
    public Transform[] castTrailTr;

    [Header("Cast")]
    public GameObject hurtboxPrefab;
    float dmg;

    [Header("Cooldown")]
    public Image radialBar;
    float cooldown=45;
    float radialFill;

    void Awake()
    {
        player=GetComponent<Player>();
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

            GameEventSystem.Current.OnAbilityCasting(gameObject, "AOE");
        }
    }
    
    bool isCasting;

    Coroutine castingRt;
    IEnumerator Casting()
    {
        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Casting);

        isCasting=true;

        ShowCastingBar();

        player.anim.CrossFade("casting", .25f, 2, 0);

        EnableCastTrails();

        castTime = UpgradeManager.Current.GetAoeCastTime();

        yield return new WaitForSeconds(castTime);

        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        isCasting=false;

        player.anim.CrossFade("aoe", .25f, 2, 0);
    }

    public void Release()
    {
        Hurtbox hurtbox = Instantiate(hurtboxPrefab, transform.position, Quaternion.identity).GetComponent<Hurtbox>();

        hurtbox.owner = gameObject;

        dmg = UpgradeManager.Current.GetAoeDmg();

        hurtbox.dmg = dmg;
        hurtbox.dmgBlock = dmg;

        float range = UpgradeManager.Current.GetAoeRange();

        SphereCollider coll = hurtbox.GetComponent<SphereCollider>();
        coll.radius = range;

        GameEventSystem.Current.OnAbilityCast(gameObject, "AOE");

        if(coolingRt!=null) StopCoroutine(coolingRt);
        coolingRt = StartCoroutine(Cooling());

        DisableCastTrails();
    }

    public void Finish()
    {
        player.sm.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        player.anim.CrossFade("cancel", .25f, 2, 0);
    }

    Coroutine coolingRt;
    IEnumerator Cooling()
    {
        cooldown = UpgradeManager.Current.GetAoeCooldown();

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

        castTime = UpgradeManager.Current.GetAoeCastTime();

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

    public void ResetCooldown()
    {
        LeanTween.cancel(tweenFillLt);
        radialFill=0;

        if(coolingRt!=null) StopCoroutine(coolingRt);
        canCast=true;
    }
}
