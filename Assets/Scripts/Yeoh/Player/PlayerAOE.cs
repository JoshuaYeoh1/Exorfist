using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAOE : MonoBehaviour
{
    Player player;
    InputBuffer buffer;

    public GameObject hitboxPrefab, castingBarPrefab, vfxPrefab;
    public Transform castingBarTr;
    
    public float castTime=1, cooldown=45;

    bool canCast=true, isCasting;

    void Awake()
    {
        player=GetComponent<Player>();
        buffer=GetComponent<InputBuffer>();
    }

    public void StartCast()
    {
        if(canCast && player.canCast)
        {
            canCast=false;

            castingRt=StartCoroutine(Casting());

            buffer.lastPressedAOE=-1;
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

        player.anim.CrossFade("aoe", .25f, 2, 0);
    }

    public void Release()
    {
        SpawnExplosion();

        StartCoroutine(Cooling());
    }

    void SpawnExplosion()
    {
        Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

        GameObject vfx = Instantiate(vfxPrefab, new Vector3(transform.position.x, transform.position.y+.5f, transform.position.z), Quaternion.identity);
        vfx.hideFlags = HideFlags.HideInHierarchy;

        Singleton.instance.CamShake(.5f, 1);
        Singleton.instance.HitStop();
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
