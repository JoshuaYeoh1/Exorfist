using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAOE : MonoBehaviour
{
    Player player;
    InputBuffer buffer;

    [Header("Casting")]
    public GameObject castingBarPrefab;
    public Transform castingBarTr;
    public float castTime=1;
    
    [Header("Trails")]
    public GameObject castTrailVFXPrefab;
    public Transform[] castTrailTr;

    [Header("Cast")]
    public GameObject hitboxPrefab;
    public GameObject explodeVFXPrefab;

    [Header("Cooldown")]
    public Image radialBar;
    public float cooldown=45;
    float radialFill;

    void Awake()
    {
        player=GetComponent<Player>();
        buffer=GetComponent<InputBuffer>();
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

            buffer.lastPressedAOE=-1;
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

        player.anim.CrossFade("aoe", .25f, 2, 0);
    }

    public void Release()
    {
        SpawnExplosion();

        StartCoroutine(Cooling());

        DisableCastTrails();
    }

    void SpawnExplosion()
    {
        Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

        GameObject vfx = Instantiate(explodeVFXPrefab, new Vector3(transform.position.x, transform.position.y+.5f, transform.position.z), Quaternion.identity);
        vfx.hideFlags = HideFlags.HideInHierarchy;

        // move to vfx manager later
        VFXManager.Current.CamShake(.5f, 3);
        VFXManager.Current.HitStop(.05f, .1f);
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
        foreach (GameObject trail in trails)
        {
            if(trail) Destroy(trail);
        }
        trails.Clear();
    }
}
