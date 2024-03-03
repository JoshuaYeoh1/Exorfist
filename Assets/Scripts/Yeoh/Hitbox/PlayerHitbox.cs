using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : BaseHitbox
{
<<<<<<< HEAD
    public bool hasSweepingEdge, camShake=true, hitStop=true;
=======
    public bool hasSweepingEdge, shake=true, hitstop=true, shockwave=true;
>>>>>>> main

    protected override bool IsTargetValid(Rigidbody otherRb)
    {
        return otherRb.tag=="Enemy";
    }

    protected override void HandleTargetHit(Rigidbody otherRb)
    {
        GameEventSystem.current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);

        ToggleActive(hasSweepingEdge); // if can swipe through multiple

<<<<<<< HEAD
        ToggleActive(hasSweepingEdge); // if can swipe through multiple

        if(camShake) Singleton.instance.CamShake();
        if(hitStop) Singleton.instance.HitStop();
        shock.SpawnShockwave(contactPoint, Color.white);
        hitmarker.SpawnHitmarker(contactPoint, Color.white);
=======


        // move to vfx manager later
        if(shake) Singleton.instance.CamShake();
        if(hitstop) Singleton.instance.HitStop();
        if(shockwave) shock.SpawnShockwave(contactPoint, Color.white);
        hitmarker.SpawnHitmarker(contactPoint, Color.white);
        GameObject impact = Instantiate(impactVFXPrefab, contactPoint, Quaternion.identity);
        impact.hideFlags = HideFlags.HideInHierarchy;
>>>>>>> main
    }
}