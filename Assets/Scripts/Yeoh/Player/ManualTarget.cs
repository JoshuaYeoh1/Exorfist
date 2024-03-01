using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTarget : MonoBehaviour
{
    public GameObject target;

    public float maxRange=10;
    public LayerMask layers;

    [Header("Leniency")]
    public float tapRadius=1;
    Vector2 startTapPos, endTapPos;
    float lastTappedTime;
    public float maxSwipeDistance = 50; // Maximum distance for a tap to be considered a swipe
    public float maxSwipeTime = 0.5f; // Maximum time for a tap to be considered a swipe

    void Update()
    {
        CheckInput();
        CheckOutOfRange();
    }

    void CheckInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startTapPos = Input.mousePosition; // Record the start position and time of the tap
            lastTappedTime = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endTapPos = Input.mousePosition;

            float swipeDistance = Vector2.Distance(startTapPos, endTapPos); // Calculate the distance moved and time taken

            if(swipeDistance<maxSwipeDistance && Time.time-lastTappedTime < maxSwipeTime) // Check if tapped
            {
                DoSphereCast();
            }
            else // Check if swiped
            {
                Vector2 swipeDirection = endTapPos - startTapPos; //Debug.Log("Swiped in direction: " + swipeDirection.normalized);
            }
        }
    }

    void DoSphereCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.SphereCast(ray, tapRadius, out RaycastHit hit, maxRange*2, layers, QueryTriggerInteraction.Collide))
        {
            Collider other = hit.collider;
            
            GameObject otherObject;

            if(other.attachedRigidbody) //if target has a rigidbody
            otherObject = other.attachedRigidbody.gameObject; 

            else otherObject = other.gameObject; //if just a collider alone

            float distance = Vector3.Distance(otherObject.transform.position, transform.position);

            if(distance<=maxRange)
            {
                if(target!=otherObject) target=otherObject;
                else target=null;
            }
        }
    }

    void CheckOutOfRange()
    {
        if(target)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if(distance>maxRange) target=null;
        }
    }
}
