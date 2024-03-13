using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManualTarget : MonoBehaviour
{
    Player player;
    public GameObject target;

    public float maxRange=10;
    public LayerMask layers;

    [Header("Leniency")]
    public float tapRadius=.5f;
    Vector2 startTapPos, endTapPos;
    float lastTappedTime;
    public float minSwipeDistance = 100; // distance for a tap to be considered a swipe
    public float minSwipeTime = 0.25f; // time for a tap to be considered a swipe

    void Awake()
    {
        player=GetComponent<Player>();
    }

    void Update()
    {
        if(player.canTarget) CheckInput();
        CheckOutOfRange();
    }

    void CheckInput()
    {
        // Check if the current pointer event is over a UI element
        if(IsPointerOverUI(Input.mousePosition)) return;

        if(Input.GetMouseButtonDown(0))
        {
            startTapPos = Input.mousePosition; // Record the start position and time of the tap
            lastTappedTime = Time.time;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            endTapPos = Input.mousePosition;

            float swipeDistance = Vector2.Distance(startTapPos, endTapPos); // Calculate the distance moved and time taken

            if(swipeDistance<minSwipeDistance && Time.time-lastTappedTime < minSwipeTime) // Check if tapped
            {
                DoSphereCast();
            }
            else // Check if swiped
            {
                Vector2 swipeVector = endTapPos-startTapPos;
                Vector2 swipeDirection = swipeVector.normalized; //Debug.Log("Swiped in direction: " + swipeDirection);
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
            else //if just a collider alone
                otherObject = other.gameObject; 

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

    List<RaycastResult> raycastResults = new List<RaycastResult>();

    bool IsPointerOverUI(Vector2 touchPos)
    {
        PointerEventData eventDataPos = new PointerEventData(EventSystem.current);

        eventDataPos.position = touchPos;

        EventSystem.current.RaycastAll(eventDataPos, raycastResults);

        if(raycastResults.Count>0) // if more than 0, then UI is touched
        {
            foreach(RaycastResult result in raycastResults)
            {
                if(result.gameObject.tag!="TouchField") return true; // ignore UI elements with this tag
            }
        }

        return false;
    }
}
