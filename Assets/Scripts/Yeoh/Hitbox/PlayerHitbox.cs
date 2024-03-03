using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : BaseHitbox
{
    public bool hasSweepingEdge, camShake=true, hitStop=true;

    protected override bool IsTargetValid(Rigidbody otherRb)
    {
        return otherRb.tag=="Enemy";
    }

    protected override void HandleTargetHit(Rigidbody otherRb)
    {
        EnemyHurt hurt = otherRb.GetComponent<EnemyHurt>();

        if(hurt) hurt.Hit(damage, knockback, contactPoint, speedDebuffMult, stunTime);

        ToggleActive(hasSweepingEdge); // if can swipe through multiple

        if(camShake) Singleton.instance.CamShake();
        if(hitStop) Singleton.instance.HitStop();
        shock.SpawnShockwave(contactPoint, Color.white);
        hitmarker.SpawnHitmarker(contactPoint, Color.white);
    }
}