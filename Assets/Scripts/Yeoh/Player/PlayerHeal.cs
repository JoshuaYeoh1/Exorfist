using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeal : MonoBehaviour
{
    Player player;
    InputBuffer buffer;
    HPManager hp;

    [Header("Casting")]
    public GameObject castingBarPrefab;
    public Transform castingBarTr;
    public float castTime=1;
    
    [Header("Trails")]
    public GameObject castTrailVFXPrefab;
    public Transform[] castTrailTr;

    [Header("Cast")]
    public GameObject healVFXPrefab;
    public GameObject shineVFXPrefab;
    public float regenTime=3, regenHp=3;
    public Material healMeshEffectMaterial;
    float defaultRegenHp;

    [Header("Cooldown")]
    public Image radialBar;
    public float cooldown=30;
    float radialFill;

    void Awake()
    {
        player=GetComponent<Player>();
        buffer=GetComponent<InputBuffer>();
        hp=GetComponent<HPManager>();

        defaultRegenHp = hp.regenHp;
    }

    void Update()
    {
        if(radialBar.IsActive()) radialBar.fillAmount = radialFill;
    }

    bool canCast=true;

    public void StartCast()
    {
        if(canCast && player.canCast && hp.hp<hp.hpMax-regenHp)
        {
            canCast=false;

            castingRt=StartCoroutine(Casting());

            buffer.lastPressedHeal=-1;
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

        yield return new WaitForSeconds(castTime);

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        isCasting=false;

        player.anim.CrossFade("heal", .25f, 2, 0);
    }

    public void Release()
    {
        DisableCastTrails();

        Heal();

        StartCoroutine(Cooling());
    }

    void Heal()
    {
        StartCoroutine(Healing());
    }

    IEnumerator Healing()
    {
        //move to vfx manager later
        GameObject shineVFX = Instantiate(shineVFXPrefab, transform.position, Quaternion.identity);
        shineVFX.hideFlags = HideFlags.HideInHierarchy;
        shineVFX.GetComponent<TransformConstraint>().constrainTo = transform;

        GameObject healVfx = Instantiate(healVFXPrefab, transform.position, Quaternion.identity);
        healVfx.hideFlags = HideFlags.HideInHierarchy;
        healVfx.GetComponent<TransformConstraint>().constrainTo = transform;

        ModelManager.Current.AddMaterial(player.playerModel, healMeshEffectMaterial);

        hp.regenHp = regenHp;
        yield return new WaitForSeconds(regenTime);
        hp.regenHp = defaultRegenHp;
        
        ModelManager.Current.RemoveMaterial(player.playerModel, healMeshEffectMaterial);
    }

    public void Finish()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);

        player.anim.CrossFade("cancel", .25f, 2, 0);
    }

    IEnumerator Cooling()
    {
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
