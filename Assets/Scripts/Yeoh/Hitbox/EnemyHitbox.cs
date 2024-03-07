using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : BaseHitbox
{
    protected override bool IsTargetValid(Rigidbody otherRb)
    {
        return otherRb.tag=="Player";
    }

    protected override void HandleTargetHit(Rigidbody otherRb)
    {
        GameEventSystem.Current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);




        // move to vfx manager later
        hitmarker.SpawnHitmarker(contactPoint, Color.red);
        GameObject impact = Instantiate(impactVFXPrefab, contactPoint, Quaternion.identity);
        impact.hideFlags = HideFlags.HideInHierarchy;
    }
}
