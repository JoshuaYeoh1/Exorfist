using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    Player player;
    InputBuffer buffer;
    ClosestObjectFinder finder;

    public GameObject hitboxPrefab, castingBarPrefab;
    public Transform castingBarTr, firepointTr;
    
    public float range=10, castTime=.25f, sustainTime=5, damageInterval=.2f, cooldown=45;

    bool canCast=true, isCasting;

    void Awake()
    {
        player=GetComponent<Player>();
        buffer=GetComponent<InputBuffer>();
        finder=GetComponent<ClosestObjectFinder>();
    }

    public void StartCast()
    {
        if(canCast && player.canCast)
        {
            canCast=false;

            castingRt=StartCoroutine(Casting());

            buffer.lastPressedLaser=-1;
        }
    }
    
    Coroutine castingRt;
    IEnumerator Casting()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Casting);

        isCasting=true;

        ShowCastingBar();

        player.anim.CrossFade("casting", .25f, 2, 0);

        yield return new WaitForSeconds(castTime);

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        isCasting=false;

        player.anim.CrossFade("laser start", .25f, 2, 0);
    }

    public void Release()
    {
        Singleton.instance.HitStop();

        StartCoroutine(Sustaining());
    }

    IEnumerator Sustaining()
    {
        SpawnLaser();

        finder.range=range;

        yield return new WaitForSeconds(sustainTime);

        if(flashingHitboxRt!=null) StopCoroutine(flashingHitboxRt);

        Destroy(laser);

        StartCoroutine(Cooling());

        player.anim.CrossFade("laser finish", .1f, 2, 0);

        finder.range=finder.defRange;
    }

    GameObject laser;
    Collider[] laserColl;
    
    void SpawnLaser()
    {
        laser = Instantiate(hitboxPrefab, firepointTr.position, firepointTr.rotation);
        laser.transform.parent = firepointTr;
        laserColl = laser.GetComponentsInChildren<Collider>();

        flashingHitboxRt = StartCoroutine(FlashingHitbox());
    }

    Coroutine flashingHitboxRt;
    IEnumerator FlashingHitbox()
    {
        while(true)
        {
            Singleton.instance.CamShake(damageInterval, 1);

            foreach(Collider coll in laserColl)
            {
                coll.enabled=true;
            }

            yield return new WaitForSeconds(damageInterval);

            foreach(Collider coll in laserColl)
            {
                coll.enabled=false;
            }
        }
    }

    public void Finish()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
    }

    IEnumerator Cooling()
    {
        yield return new WaitForSeconds(cooldown);
        canCast=true;
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
}
