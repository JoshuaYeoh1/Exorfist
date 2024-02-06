using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    protected GameObject owner;
    Hitmarker hitmarker;
    ShockwaveVFX shock;

    public bool enabledOnAwake;
    public float damage, knockback;
    public float speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge;

    protected Vector3 contactPoint;
    protected Color hitmarkerColor, shockwaveColor;

    void Awake()
    {
        owner=transform.root.gameObject;
        hitmarker=GetComponent<Hitmarker>();
        shock=GetComponent<ShockwaveVFX>();

        ToggleActive(enabledOnAwake);
    }

    public void ToggleActive(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    void OnTriggerEnter(Collider other)
    {
        contactPoint = other.ClosestPointOnBounds(transform.position);

        Rigidbody otherRb = other.attachedRigidbody;

        hitmarkerColor = shockwaveColor = Color.white;

        if(otherRb && IsTargetValid(otherRb))
        {
            HandleTargetHit(otherRb);

            ToggleActive(hasSweepingEdge); // if can swipe through multiple
        }

        hitmarker.SpawnHitmarker(contactPoint, hitmarkerColor);

        shock.SpawnShockwave(contactPoint, shockwaveColor);
    }

    protected virtual bool IsTargetValid(Rigidbody otherRb)
    {
        return false;
    }

    protected virtual void HandleTargetHit(Rigidbody otherRb)
    {
        //print("dmg: " + damage + " | kb: " + knockback);
    }
}
