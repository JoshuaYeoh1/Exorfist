using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject hitmarker;

    public float damage, knockback;

    void Awake()
    {
        ToggleActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.tag=="Enemy")
        {
            print("dmg: " + damage + " | kb: " + knockback);

            ToggleActive(false); // optional, no swiping through multiple enemies
        }

        GameObject spawnedHitmarker = Instantiate(hitmarker, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
        Destroy(spawnedHitmarker, .1f);
    }

    public void ToggleActive(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
