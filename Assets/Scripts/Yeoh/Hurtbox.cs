using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    Collider coll;
    public GameObject owner;

    public bool enabledOnAwake;
    public float dmg, kbForce;
    public float speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge=true;

    void Awake()
    {
        coll = GetComponent<Collider>();
        if(!owner) owner = gameObject;

        ToggleActive(enabledOnAwake);
    }

    [HideInInspector] public Vector3 contactPoint;

    void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            Rigidbody otherRb = other.attachedRigidbody;

            if(otherRb) Hit(other, otherRb);
        }
    }

    public Transform hitboxOrigin;

    void Hit(Collider other, Rigidbody otherRb)
    {
        if(hitboxOrigin) contactPoint = other.ClosestPointOnBounds(hitboxOrigin.position);
        else contactPoint = other.ClosestPointOnBounds(transform.position);

        ToggleActive(hasSweepingEdge); // if can swipe through multiple
        
        GameEventSystem.Current.OnHit(owner, otherRb.gameObject, GetHurtInfoCopy());
    }

    HurtInfo GetHurtInfoCopy()
    {
        HurtInfo newHurtInfo = new HurtInfo();

        newHurtInfo.coll = coll;
        newHurtInfo.owner = owner;
        newHurtInfo.dmg = dmg;
        newHurtInfo.kbForce = kbForce;
        newHurtInfo.contactPoint = contactPoint;
        newHurtInfo.speedDebuffMult = speedDebuffMult;
        newHurtInfo.stunTime = stunTime;
        newHurtInfo.hasSweepingEdge = hasSweepingEdge;
        newHurtInfo.doImpact = doImpact;
        newHurtInfo.doShake = doShake;
        newHurtInfo.doHitstop = doHitstop;
        newHurtInfo.doShockwave = doShockwave;

        return newHurtInfo;
    }

    public void BlinkHitbox(float time=.1f)
    {
        if(time>0)
        {
            if(blinkingHitboxRt!=null) StopCoroutine(blinkingHitboxRt);
            blinkingHitboxRt = StartCoroutine(BlinkingHitbox(time)); 
        }
    }

    Coroutine blinkingHitboxRt;
    IEnumerator BlinkingHitbox(float t)
    {
        ToggleActive(true);
        yield return new WaitForSeconds(t);
        ToggleActive(false);
    }

    public void ToggleActive(bool toggle)
    {
        coll.enabled=toggle;
    }

    public bool doImpact=true, doShake=true, doHitstop=true, doShockwave=true;
}
