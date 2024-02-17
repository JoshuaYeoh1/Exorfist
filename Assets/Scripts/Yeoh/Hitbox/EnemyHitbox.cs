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

        if (block.isParrying)
        {
            EnemyAI thisEnemy = owner.GetComponent<EnemyAI>();
            if (thisEnemy != null)
            {
                thisEnemy.sm.SwitchState(thisEnemy.sm.hitStunState);
            }
            else
            {
                Debug.Log("Enemy that got parried does not have a EnemyAI script attached");
                return;
            }
        }

        //if(block.isParrying) //owner.Stagger(staggerTime); // enemy got parried

        hitmarkerColor = Color.red;
    }
}
