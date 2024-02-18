using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCataclysmWave : MonoBehaviour
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

            isCasting=true;

            castingRt=StartCoroutine(Casting());

            buffer.lastPressedAOE=-1;
        }
    }
    
    Coroutine castingRt;
    IEnumerator Casting()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Casting);

        ShowCastingBar();

        player.anim.CrossFade("casting", .25f, 2, 0);

        yield return new WaitForSeconds(castTime);

        player.anim.CrossFade("cataclysm wave", .25f, 2, 0);
    }

    public void Release()
    {
        isCasting=false;

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Cast);

        Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

        coolingRt=StartCoroutine(Cooling());

        Singleton.instance.CamShake(.5f, 1);
        Singleton.instance.HitStop();

        GameObject vfx = Instantiate(vfxPrefab, new Vector3(transform.position.x, transform.position.y+.5f, transform.position.z), Quaternion.identity);
        vfx.hideFlags = HideFlags.HideInHierarchy;
    }

    public void Finish()
    {
        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Idle);
    }

    Coroutine coolingRt;
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

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) StartCast();
    }
}
