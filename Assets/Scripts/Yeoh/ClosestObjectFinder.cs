using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestObjectFinder : MonoBehaviour
{
    public GameObject target;

    public float range=5;
    public LayerMask layers;

    [HideInInspector] public float defRange;
    
    void Awake()
    {
        defRange=range;
    }

    void Update()
    {
        Collider[] targets =  Physics.OverlapSphere(transform.position, range, layers);

        if(targets.Length<=0)
        {
            if(target) target=null; // no target if nothing is in range
        }
        else
        {
            GameObject closestTarget = null;

            float closestDistance = Mathf.Infinity;

            foreach(Collider other in targets) // go through all detected colliders
            {
                float distance;

                if(other.attachedRigidbody) //if target has a rigidbody
                {
                    distance = Vector3.Distance(other.attachedRigidbody.transform.position, transform.position);

                    if(distance < closestDistance) // find and replace with the nearer one
                    {
                        closestDistance = distance;

                        closestTarget = other.attachedRigidbody.gameObject;
                    }
                }
                else if(other.transform.root) //if target has a parent
                {
                    distance = Vector3.Distance(other.transform.root.position, transform.position);

                    if(distance < closestDistance) // find and replace with the nearer one
                    {
                        closestDistance = distance;

                        closestTarget = other.transform.root.gameObject;
                    }
                }
                else //if just a collider alone
                {
                    distance = Vector3.Distance(other.transform.position, transform.position);

                    if(distance < closestDistance) // find and replace with the nearer one
                    {
                        closestDistance = distance;

                        closestTarget = other.gameObject;
                    }
                }
            }

            if(target!=closestTarget) target = closestTarget;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
