using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoller : MonoBehaviour
{
    public Animator mainAnimator;
    public Collider mainColl;
    public Rigidbody mainRb;

    [Header("Skeleton")]
    public GameObject rigParent;
    public Transform rigHips;
    public float ragdollMass=1;
    public bool ragdollOnAwake;

    Collider[] rigColls;
    Rigidbody[] rigRbs;

    void Awake()
    {
        rigColls = rigParent.GetComponentsInChildren<Collider>();
        rigRbs = rigParent.GetComponentsInChildren<Rigidbody>();

        ToggleRagdoll(ragdollOnAwake);
    }

    public void ToggleRagdoll(bool toggle=true)
    {
        if(mainAnimator) mainAnimator.enabled=!toggle;
        if(mainColl) mainColl.enabled=!toggle;
        if(mainRb) mainRb.isKinematic=toggle;

        foreach(Collider coll in rigColls)
        {
            coll.enabled=toggle;
        }
        foreach(Rigidbody rb in rigRbs)
        {
            rb.isKinematic=!toggle;

            if(ragdollMass>0) rb.mass=ragdollMass;
            else rb.useGravity=false;
        }

        isRagdoll=toggle;

        if(!toggle) AlignToRagdoll();
    }

    bool isRagdoll;

    void AlignToRagdoll()
    {
        Vector3 originalHipsPos = rigHips.position; // world space
        transform.position = originalHipsPos;
        rigHips.position = originalHipsPos;
    }

    public void AlignToFloor()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    public void PushRagdoll(float force, Vector3 contactPoint, float radiusMult=.5f)
    {
        if(isRagdoll)
        {
            foreach(Rigidbody rb in rigRbs)
            {
                rb.AddExplosionForce(force, contactPoint, force*radiusMult, 0, ForceMode.Impulse);
            }
        }
    }
}
