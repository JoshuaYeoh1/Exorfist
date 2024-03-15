using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    ClosestObjectFinder finder;

    public float magnetDelay=1, magnetTime=.5f;
    public Vector3 targetPosOffset;

    void Awake()
    {
        finder=GetComponent<ClosestObjectFinder>();
    }

    bool canMagnet;

    void OnEnable()
    {
        if(magnetDelay>0) StartCoroutine(MagnetDelaying());
    }

    IEnumerator MagnetDelaying()
    {
        canMagnet=false;
        yield return new WaitForSeconds(magnetDelay);
        canMagnet=true;
    }

    void FixedUpdate()
    {
        if(finder.target && canMagnet)
        {
            SmoothTowards(transform.position, finder.target.transform.position+targetPosOffset, magnetTime);
        }
    }

    Vector3 velocity;

    void SmoothTowards(Vector3 from, Vector3 to, float moveTime)
    {
        transform.position = Vector3.SmoothDamp(from, to, ref velocity, moveTime);
    }
}
