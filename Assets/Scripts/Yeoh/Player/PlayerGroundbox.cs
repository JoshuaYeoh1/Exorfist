using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundbox : MonoBehaviour
{
    public bool isGrounded;

    int collCount;

    void OnTriggerEnter(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerEnter(other, gameObject, OnTriggerExit);

        collCount++;

        if(!other.isTrigger && collCount>0)
        {
            isGrounded=true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        ReliableOnTriggerExit.NotifyTriggerExit(other, gameObject);

        if(collCount>0) collCount--;

        if(!other.isTrigger && collCount<1)
        {
            isGrounded=false;
        }
    }
}
