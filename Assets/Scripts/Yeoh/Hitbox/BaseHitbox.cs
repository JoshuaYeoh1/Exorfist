using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    protected GameObject owner;
    Hitmarker hitmarker;
    protected ShockwaveVFX shock;
    Collider coll;

    public bool enabledOnAwake;
    public float damage, knockback;
    public float speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge;

    protected Vector3 contactPoint;
    protected Color hitmarkerColor;

    void Awake()
    {
        owner=transform.root.gameObject;
        hitmarker=GetComponent<Hitmarker>();
        shock=GetComponent<ShockwaveVFX>();
        coll=GetComponent<Collider>();

        ToggleActive(enabledOnAwake);
    }

    public void ToggleActive(bool toggle)
    {
        coll.enabled=toggle;
    }

    void OnTriggerEnter(Collider other)
    {
        contactPoint = other.ClosestPointOnBounds(transform.position);

        Rigidbody otherRb = other.attachedRigidbody;

        hitmarkerColor = Color.white;

        if(otherRb && IsTargetValid(otherRb))
        {
            HandleTargetHit(otherRb);

            ToggleActive(hasSweepingEdge); // if can swipe through multiple
        }

        hitmarker.SpawnHitmarker(contactPoint, hitmarkerColor);
    }

    protected virtual bool IsTargetValid(Rigidbody otherRb)
    {
        return false;
    }

    protected virtual void HandleTargetHit(Rigidbody otherRb)
    {
        //print("dmg: " + damage + " | kb: " + knockback);
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
