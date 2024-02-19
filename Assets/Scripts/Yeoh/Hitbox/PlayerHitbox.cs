using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : BaseHitbox
{
    protected override bool IsTargetValid(Rigidbody otherRb)
    {
        return otherRb.tag=="Enemy";
    }

    protected override void HandleTargetHit(Rigidbody otherRb)
    {
        EnemyHurt hurt = otherRb.GetComponent<EnemyHurt>();

        if(hurt) hurt.Hit(damage, knockback, contactPoint, speedDebuffMult, stunTime);

        shock.SpawnShockwave(contactPoint, Color.white);
        hitmarker.SpawnHitmarker(contactPoint, Color.white);
    }
}