using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    HPManager hp;
    Rigidbody rb;

    bool iframe;
    public float iframeTime=.1f;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        rb=GetComponent<Rigidbody>();
    }

    public void Hurt(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(!iframe)
        {
            DoIFraming(iframeTime, .5f, -.5f, -.5f); // flicker red

            Knockback(kbForce, contactPoint);

            GameEventSystem.current.OnHurt(gameObject, attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime);

            hp.Hit(dmg);

            if(hp.hp>0) // if still alive
            {
                //stun.Stun(speedDebuffMult, stunTime);
            }
            else Die(attacker, dmg, kbForce, contactPoint);



            // move to vfx manager later
            VFXManager.current.SpawnPopUpText(contactPoint, dmg.ToString(), Color.white);
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
            ModelManager.current.OffsetColor(gameObject, r, g, b);
            yield return new WaitForSecondsRealtime(.05f);
            ModelManager.current.RevertColor(gameObject);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }

    void StopIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        ModelManager.current.RevertColor(gameObject);
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

    void Die(GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        ModelManager.current.RevertColor(gameObject);

        GameEventSystem.current.OnDeath(gameObject, killer, dmg, kbForce, contactPoint);
    }
}
