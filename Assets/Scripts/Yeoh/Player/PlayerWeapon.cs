using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject hitmarker;

    public float damage, knockback;
    public bool hasSweepingEdge;

    void Awake()
    {
        ToggleActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && otherRb.tag=="Enemy")
        {
            EnemyHurt enemy = otherRb.GetComponent<EnemyHurt>();

            enemy.Hit(damage);

            Vector3 kbVector = otherRb.transform.position - contactPoint;
            kbVector.y=0;
            
            otherRb.velocity = new Vector3(0, otherRb.velocity.y, 0);
            otherRb.AddForce(kbVector.normalized * knockback, ForceMode.Impulse);

            ToggleActive(hasSweepingEdge); // if can swiping through multiple enemies

            //Singleton.instance.camShake();

            //Singleton.instance.FadeTimeTo(float to, float time, float delay=0);

            //print("dmg: " + damage + " | kb: " + knockback);
        }

        GameObject spawnedHitmarker = Instantiate(hitmarker, contactPoint, Quaternion.identity);
        Destroy(spawnedHitmarker, .1f);
    }

    public void ToggleActive(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
