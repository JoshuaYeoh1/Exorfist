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
        
        if(block.isParrying)
        {
            //owner.Stagger(staggerTime);

            block.ParrySuccess(contactPoint);

            hitmarkerColor = shockwaveColor = Color.green;
        }
        else if(block.isBlocking)
        {
            block.BlockHit(damage, knockback, contactPoint, speedDebuffMult, stunTime);

            hitmarkerColor = shockwaveColor = Color.cyan;
        }
        else
        {
            PlayerHurt hurt = otherRb.GetComponent<PlayerHurt>();

            hurt.Hit(damage, knockback, contactPoint, speedDebuffMult, stunTime);

            hitmarkerColor = shockwaveColor = Color.red;
        }
    }
}
