using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    Player player;
    HPManager hp;
    Rigidbody rb;
    PlayerStun stun;

    public bool iframe;
    public float iframeTime=.5f;

    void Awake()
    {
        player=GetComponent<Player>();
        hp=GetComponent<HPManager>();
        rb=GetComponent<Rigidbody>();
        stun=GetComponent<PlayerStun>();
    }

    public void Hurt(GameObject attacker, HurtInfo hurtInfo)
    {
        if(!iframe && player.isAlive && player.canHurt)
        {
            DoIFraming(iframeTime, .5f, -.5f, -.5f); // flicker red

            Knockback(hurtInfo.kbForce, hurtInfo.contactPoint);

            GameEventSystem.Current.OnHurt(gameObject, attacker, hurtInfo);

            hp.Hit(hurtInfo.dmg);

            if(hp.hp>0) // if still alive
            {
                stun.Stun(hurtInfo.speedDebuffMult, hurtInfo.stunTime);

                // move to vfx manager later   
                // flash screen red
            }
            else Die(attacker, hurtInfo);





            // move to vfx manager later
            // stay red screen
            //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
        }
    }

    public void DoIFraming(float t, float r, float g, float b)
    {
        if(iFramingRt!=null) StopCoroutine(iFramingRt);
        iFramingRt = StartCoroutine(IFraming(t, r, g, b));
    }

    Coroutine iFramingRt;
    IEnumerator IFraming(float t, float r, float g, float b)
    {
        iframe=true;
        StartIFrameFlicker(r, g, b);

        yield return new WaitForSeconds(t);

        iframe=false;
        StopIFrameFlicker();
    }

    void StartIFrameFlicker(float r, float g, float b)
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        iFrameFlickeringRt = StartCoroutine(IFrameFlickering(r, g, b));
    }

    Coroutine iFrameFlickeringRt;
    IEnumerator IFrameFlickering(float r, float g, float b)
    {
        while(true)
        {
            ModelManager.Current.OffsetColor(gameObject, r, g, b);
            yield return new WaitForSecondsRealtime(.05f);
            ModelManager.Current.RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    void StopIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        ModelManager.Current.RevertColor(gameObject);
    }

    public void Knockback(float force, Vector3 contactPoint)
    {
        if(force>0)
        {
            Vector3 kbVector = rb.transform.position - contactPoint;
            kbVector.y = 0;

            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            rb.AddForce(kbVector.normalized * force, ForceMode.Impulse);
        }
    }

    void Die(GameObject killer, HurtInfo hurtInfo)
    {
        ModelManager.Current.RevertColor(gameObject);
        
        player.CancelActions();

        GameEventSystem.Current.OnDeath(gameObject, killer, hurtInfo);

        player.stateMachine.TransitionToState(PlayerStateMachine.PlayerStates.Death);
    }
}
