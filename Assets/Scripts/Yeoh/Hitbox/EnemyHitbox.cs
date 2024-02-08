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
        PlayerBlock block = otherRb.GetComponent<PlayerBlock>();

        if(block) block.CheckBlock(damage, knockback, contactPoint, speedDebuffMult, stunTime);
        
        //if(block.isParrying) //owner.Stagger(staggerTime); // enemy got parried

        hitmarkerColor = Color.red;
    }
}
