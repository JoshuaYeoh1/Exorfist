// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerHitbox : BaseHitbox
// {
//     protected override bool IsTargetValid(Rigidbody otherRb)
//     {
//         return otherRb.tag=="Enemy";
//     }

//     protected override void HandleTargetHit(Rigidbody otherRb)
//     {
//         GameEventSystem.Current.OnHit(owner, otherRb.gameObject, damage, knockback, contactPoint, speedDebuffMult, stunTime);

//         ToggleActive(hasSweepingEdge); // if can swipe through multiple
//     }
// }