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

        hurt.Hit(damage, knockback, contactPoint, speedDebuffMult, stunTime);
    }
}