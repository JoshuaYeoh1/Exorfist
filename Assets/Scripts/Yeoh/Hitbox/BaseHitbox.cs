using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitbox : MonoBehaviour
{
    [HideInInspector] public GameObject owner;
    public GameObject hitmarker;

    public bool enabledOnAwake;
    public float damage, knockback;
    public float speedDebuffMult=.3f, stunTime=.5f;
    public bool hasSweepingEdge;

    protected Vector3 contactPoint;

    void Awake()
    {
        owner = transform.root.gameObject;

        ToggleActive(enabledOnAwake);
    }

    public void ToggleActive(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    void OnTriggerEnter(Collider other)
    {
        contactPoint = other.ClosestPointOnBounds(transform.position);

        Rigidbody otherRb = other.attachedRigidbody;

        if(otherRb && IsTargetValid(otherRb))
        {
            HandleTargetHit(otherRb);

            ToggleActive(hasSweepingEdge); // if can swipe through multiple

            //print("dmg: " + damage + " | kb: " + knockback);
        }

        SpawnHitmarker();
    }

    void SpawnHitmarker()
    {
        GameObject spawnedHitmarker = Instantiate(hitmarker, contactPoint, Quaternion.identity);
        Destroy(spawnedHitmarker, 0.1f);
    }

    protected virtual bool IsTargetValid(Rigidbody otherRb)
    {
        return false;
    }

    protected virtual void HandleTargetHit(Rigidbody otherRb)
    {

    }
}
