using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    Collider coll;
    public GameObject owner;

    public string ownerName;
    public string attackName;

    public bool enabledOnAwake;
    public float dmg=1, dmgBlock=1, kbForce=1;
    public float speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge=true, unparryable;

    void Awake()
    {
        coll = GetComponent<Collider>();

        ToggleActive(enabledOnAwake);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            Rigidbody otherRb = other.attachedRigidbody;

            if(otherRb) Hit(other, otherRb);
        }
    }

    public Transform hitboxOrigin;
    [HideInInspector] public Vector3 contactPoint;

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
        newHurtInfo.attackerName = ownerName;
        newHurtInfo.attackName = attackName;
        newHurtInfo.dmg = dmg;
        newHurtInfo.dmgBlock = dmgBlock;
        newHurtInfo.kbForce = kbForce;
        newHurtInfo.contactPoint = contactPoint;
        newHurtInfo.speedDebuffMult = speedDebuffMult;
        newHurtInfo.stunTime = stunTime;
        newHurtInfo.hasSweepingEdge = hasSweepingEdge;
        newHurtInfo.doImpact = doImpact;
        newHurtInfo.doShake = doShake;
        newHurtInfo.doHitstop = doHitstop;
        newHurtInfo.doShockwave = doShockwave;
        newHurtInfo.unparryable = unparryable;

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
