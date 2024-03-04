using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    HPManager hp;
    OffsetMeshColor color;
    Rigidbody rb;

    bool iframe;
    public float iframeTime=.1f;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        color=GetComponent<OffsetMeshColor>();
        rb=GetComponent<Rigidbody>();
    }

    public void Hurt(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult=.3f, float stunTime=.5f)
    {
        if(!iframe)
        {
            DoIFraming(iframeTime);

            Knockback(kbForce, contactPoint);

            GameEventSystem.current.OnHurt(gameObject, attacker, dmg, kbForce, contactPoint, speedDebuffMult, stunTime);

            hp.Hit(dmg);

            if(hp.hp>0) // if still alive
            {
                //stun.Stun(speedDebuffMult, stunTime);

                Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.white);
            }
            else Die(attacker, dmg, kbForce, contactPoint);



            // move to vfx manager later
            Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.white);
        }
    }

    public void DoIFraming(float t)
    {
        StartCoroutine(IFraming(t));
    }
    IEnumerator IFraming(float t)
    {
        iframe=true;

        StartIFrameFlicker();

        yield return new WaitForSeconds(t);

        iframe=false;

        StopIFrameFlicker();
    }

    void StartIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        iFrameFlickeringRt = StartCoroutine(IFrameFlickering());
    }
    void StopIFrameFlicker()
    {
        if(iFrameFlickeringRt!=null) StopCoroutine(iFrameFlickeringRt);
        color.OffsetColor();
    }

    Coroutine iFrameFlickeringRt;
    IEnumerator IFrameFlickering()
    {
        while(true)
        {
            color.OffsetColor(.5f, -.5f, -.5f);
            yield return new WaitForSecondsRealtime(.05f);
            color.OffsetColor();
            yield return new WaitForSecondsRealtime(.05f);
        }
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
        GameEventSystem.current.OnDeath(gameObject, killer, dmg, kbForce, contactPoint);
    }
}
