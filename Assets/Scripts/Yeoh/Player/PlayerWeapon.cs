using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public BoxCollider hitbox;

    public float damage, knockback;

    void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody.tag=="Enemy")
        {

        }
    }

    public void ToggleTriggerBox(bool toggle)
    {
        hitbox.enabled = toggle;
    }
}
