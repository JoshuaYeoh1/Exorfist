using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightChecker : MonoBehaviour
{
    public float lineRadius=.01f;

    public LayerMask obstacleLayers;

    public bool HasLineOfSight(Vector3 from, Vector3 target, float fromYOffset=0)
    {
        from.y += fromYOffset;

        Vector3 direction = (target-from).normalized;

        if(Physics.SphereCast(from, lineRadius, direction, out RaycastHit hit, Mathf.Infinity, obstacleLayers))
        {
            float distanceToHit = Vector3.Distance(from, hit.point);
            float distanceToTarget = Vector3.Distance(from, target);

            if(distanceToHit > distanceToTarget)
            {
                return true; // obstacle is behind target
            }
        }
        else return true; // no obstacle hit

        return false; // if neither
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, lineRadius);
    }
}
