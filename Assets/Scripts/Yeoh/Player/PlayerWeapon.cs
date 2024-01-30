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
        if(other.attachedRigidbody.tag=="Enemy")
        {
            print("dmg: " + damage + " | kb: " + knockback);

            GameObject spawnedHitmarker = Instantiate(hitmarker, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
            Destroy(spawnedHitmarker, .1f);
        }
    }

    public void ToggleActive(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
