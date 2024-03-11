using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    public GameObject owner;
    public Transform hitboxOrigin;
    protected Hitmarker hitmarker;
    protected ShockwaveVFX shock;
    Collider coll;
    public GameObject impactVFXPrefab;

    public bool enabledOnAwake;
    public float damage, knockback;
    public float speedDebuffMult=.3f, stunTime=.5f;

    public Vector3 contactPoint;

    void Awake()
    {
        if(!owner) owner = gameObject;
        
        hitmarker = GetComponent<Hitmarker>();
        shock = GetComponent<ShockwaveVFX>();
        coll = GetComponent<Collider>();

        ToggleActive(enabledOnAwake);
    }

    public void ToggleActive(bool toggle)
    {
        coll.enabled=toggle;
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;

        if(hitboxOrigin)
        {
            contactPoint = other.ClosestPointOnBounds(hitboxOrigin.position);
        }
        else
        {
            contactPoint = other.ClosestPointOnBounds(transform.position);
        }

        if(otherRb && IsTargetValid(otherRb))
        {
            HandleTargetHit(otherRb);
        }
    }

    protected virtual bool IsTargetValid(Rigidbody otherRb)
    {
        return false;
    }

    protected virtual void HandleTargetHit(Rigidbody otherRb)
    {
        //Debug.Log($"dmg: {damage} | kb: {knockback}");
    }

    public void BlinkHitbox(float time)
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
}
