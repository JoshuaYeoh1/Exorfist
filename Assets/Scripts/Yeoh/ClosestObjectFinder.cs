using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestObjectFinder : MonoBehaviour
{
    public GameObject target;

    public float range=5;
    public LayerMask layers;

    void Update()
    {
        Collider[] collidersToDmg =  Physics.OverlapSphere(transform.position, range, layers);

        if(collidersToDmg.Length<=0)
        {
            if(target) target=null; // no target if nothing is in range
        }
        else
        {
            GameObject closestItem = null;

            float closestDistance = Mathf.Infinity;

            foreach(Collider other in collidersToDmg) // go through all detected colliders
            {
                float distance = Vector3.Distance(other.attachedRigidbody.transform.position, transform.position);

                if(distance < closestDistance) // find and replace with the nearer one
                {
                    closestDistance = distance;

                    closestItem = other.attachedRigidbody.gameObject;
                }
            }

            if(target!=closestItem) target = closestItem;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
