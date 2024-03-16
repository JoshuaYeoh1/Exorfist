using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundbox : MonoBehaviour
{
    public bool isGrounded;

    int collCount;

    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger) return;

        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        collCount++;

        if(collCount>0)
        {
            isGrounded=true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.isTrigger) return;

        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        if(collCount>0) collCount--;

        if(collCount<1)
        {
            isGrounded=false;
        }
    }
}
