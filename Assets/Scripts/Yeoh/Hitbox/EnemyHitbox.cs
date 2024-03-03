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
        GameEventSystem.current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);

<<<<<<< HEAD
        if(block)
        {
            block.CheckBlock(damage, knockback, contactPoint, speedDebuffMult, stunTime);

            if(block.isParrying)
            {
                EnemyAI thisEnemy = owner.GetComponent<EnemyAI>();
                
                if(thisEnemy) thisEnemy.sm.SwitchState(thisEnemy.sm.hitStunState);
            }
        }

        hitmarker.SpawnHitmarker(contactPoint, Color.red);
=======



        // move to vfx manager later
        hitmarker.SpawnHitmarker(contactPoint, Color.red);
        GameObject impact = Instantiate(impactVFXPrefab, contactPoint, Quaternion.identity);
        impact.hideFlags = HideFlags.HideInHierarchy;
>>>>>>> main
    }
}
