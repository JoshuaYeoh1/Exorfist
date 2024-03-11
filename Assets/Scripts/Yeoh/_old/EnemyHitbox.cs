// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnemyHitbox : BaseHitbox
// {
//     protected override bool IsTargetValid(Rigidbody otherRb)
//     {
//         return otherRb.tag=="Player";
//     }

//     protected override void HandleTargetHit(Rigidbody otherRb)
//     {
//         GameEventSystem.Current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);
//     }
// }
