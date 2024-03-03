using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestObjectFinder : MonoBehaviour
{
    public GameObject target;
    
    GameObject innerTarget, outerTarget;
    public float  innerRange=3, outerRange=5;
    public LayerMask layers;

    [HideInInspector] public float defRange;
    
    void Awake()
    {
        defRange=outerRange;
    }

    void Update()
    {
        ScanInnerRange();
        ScanOuterRange();

        // prioritize the inner target
        if(innerTarget) target = innerTarget;
        else if(outerTarget) target = outerTarget;
        else target=null;
    }

    void ScanInnerRange()
    {
        Collider[] targets =  Physics.OverlapSphere(transform.position, innerRange, layers);

        if(targets.Length>0)
        {
            if(!innerTarget) innerTarget = GetClosestObject(targets);
        }
        else // if nothing is in range
        {
            if(innerTarget) innerTarget=null; 
        }

        if(innerTarget)
        {
            float distance = Vector3.Distance(innerTarget.transform.position, transform.position);

            if(distance>innerRange) innerTarget=null;
        }
    }

    void ScanOuterRange()
    {
        Collider[] targets =  Physics.OverlapSphere(transform.position, outerRange, layers);
        
        if(targets.Length>0)
        {
            outerTarget = GetClosestObject(targets);
        }
        else // if nothing is in range
        {
            if(outerTarget) outerTarget=null; 
        }
    }

    GameObject GetClosestObject(Collider[] others)
    {
        GameObject closestObject = null;

        float closestDistance = Mathf.Infinity;

        foreach(Collider other in others) // go through all detected colliders
        {
            GameObject otherObject;

            if(other.attachedRigidbody) //if target has a rigidbody
                otherObject = other.attachedRigidbody.gameObject; 

            else //if just a collider alone
                otherObject = other.gameObject; 

            float distance = Vector3.Distance(otherObject.transform.position, transform.position);

            if(distance<closestDistance) // find and replace with the nearer one
            {
                closestDistance = distance;
                closestObject = otherObject;
            }
        }

        return closestObject;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, .5f);
        Gizmos.DrawWireSphere(transform.position, innerRange);
        Gizmos.DrawWireSphere(transform.position, outerRange);
    }
}
