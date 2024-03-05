using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : BaseHitbox
{
    public bool hasSweepingEdge, shake=true, hitstop=true, shockwave=true;

    protected override bool IsTargetValid(Rigidbody otherRb)
    {
        return otherRb.tag=="Enemy";
    }

    protected override void HandleTargetHit(Rigidbody otherRb)
    {
        GameEventSystem.current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);

        ToggleActive(hasSweepingEdge); // if can swipe through multiple



        // move to vfx manager later
        if(shake) VFXManager.current.CamShake();
        if(hitstop) VFXManager.current.HitStop();
        if(shockwave) shock.SpawnShockwave(contactPoint, Color.white);
        hitmarker.SpawnHitmarker(contactPoint, Color.white);
        GameObject impact = Instantiate(impactVFXPrefab, contactPoint, Quaternion.identity);
        impact.hideFlags = HideFlags.HideInHierarchy;
    }
}