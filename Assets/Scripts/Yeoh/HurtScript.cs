using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtScript : MonoBehaviour
{
    Rigidbody rb;
    HPManager hp;

    public string subjectName;

    void Awake()
    {
        rb=GetComponent<Rigidbody>();
        hp=GetComponent<HPManager>();

        maxPoise=poise;
    }

    void OnEnable()
    {
        GameEventSystem.Current.RespawnEvent += OnRespawn;
    }
    void OnDisable()
    {
        GameEventSystem.Current.RespawnEvent -= OnRespawn;
    }

    void Update()
    {
        CheckPoiseRegen();
    }

    // check block before hurt

    public void Hurt(GameObject attacker, HurtInfo hurtInfo)
    {
        if(iframe) return;

        hp.Hit(hurtInfo.dmg);

        if(hp.hp>0) // if still alive
        {
            DoIFraming(iframeTime, .5f, -.5f, -.5f); // flicker red

            HurtPoise(attacker, hurtInfo);
        }
        else
        {
            Die(attacker, hurtInfo);
        }

        hurtInfo.victimName = subjectName;

        GameEventSystem.Current.OnHurt(gameObject, attacker, hurtInfo);

        // move to sfx manager later
        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }

    [Header("iFrame")]
    public bool iframe;
    public float iframeTime=.5f;

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

    [Header("Poise")]
    public float poise;
    float maxPoise;

    public void HurtPoise(GameObject attacker, HurtInfo hurtInfo)
    {
        poise-=hurtInfo.dmg;

        lastPoiseDmgTime=Time.time;

        if(poise<=0)
        {
            poise=maxPoise;

            hurtInfo.victimName = subjectName;

            GameEventSystem.Current.OnStun(gameObject, attacker, hurtInfo);

            Knockback(hurtInfo.kbForce, hurtInfo.contactPoint);
        }
    }

    float lastPoiseDmgTime;
    public float poiseRegenDelay=3;
    
    void CheckPoiseRegen()
    {
        if(Time.time-lastPoiseDmgTime > poiseRegenDelay)
        {
            if(poise<maxPoise)
            {
                poise=maxPoise; // instant fill instead of slowly regen
            }
        }
    }

    public void Knockback(float force, Vector3 contactPoint)
    {
        if(force<=0) return;

        Vector3 kbVector = rb.transform.position - contactPoint;
        kbVector.y = 0; // only horizontal force

        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        rb.AddForce(kbVector.normalized * force, ForceMode.Impulse);
    }

    void Die(GameObject killer, HurtInfo hurtInfo)
    {
        ModelManager.Current.RevertColor(gameObject);
        
        hurtInfo.victimName = subjectName;

        GameEventSystem.Current.OnDeath(gameObject, killer, hurtInfo);
    }

    void OnRespawn(GameObject zombo)
    {
        if(zombo!=gameObject) return;
        
        hp.hp = hp.hpMax*.5f;
    }
}
