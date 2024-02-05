// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerWeapon : MonoBehaviour
// {
//     GameObject owner;
//     public GameObject hitmarker;

//     public float damage, knockback;
//     public bool hasSweepingEdge;

//     Vector3 contactPoint;

//     void Awake()
//     {
//         owner=transform.root.gameObject;

//         ToggleActive(false);
//     }

//     void OnTriggerEnter(Collider other)
//     {
//         contactPoint = other.ClosestPointOnBounds(transform.position);

//         Rigidbody otherRb = other.attachedRigidbody;

//         if(otherRb && otherRb.tag=="Enemy")
//         {
//             EnemyHurt hurt = otherRb.GetComponent<EnemyHurt>();

//             hurt.Hit(damage);

//             Knockback(otherRb);

//             ToggleActive(hasSweepingEdge); // if can swipe through multiple enemies

//             //Singleton.instance.camShake();

//             //Singleton.instance.FadeTimeTo(float to, float time, float delay=0);

//             //print("dmg: " + damage + " | kb: " + knockback);
//         }

//         GameObject spawnedHitmarker = Instantiate(hitmarker, contactPoint, Quaternion.identity);
//         Destroy(spawnedHitmarker, .1f);
//     }

//     public void ToggleActive(bool toggle)
//     {
//         gameObject.SetActive(toggle);
//     }

//     void Knockback(Rigidbody rb)
//     {
//         Vector3 kbVector = rb.transform.position - contactPoint;
//         kbVector.y=0;
        
//         rb.velocity = new Vector3(0, rb.velocity.y, 0);
//         rb.AddForce(kbVector.normalized * knockback, ForceMode.Impulse);
//     }
// }
