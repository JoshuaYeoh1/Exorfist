using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool destroyOnStart;
    public float minTime=3, maxTime=4, shrinkTime=.5f;

    void Start()
    {
        if(destroyOnStart) Destroyy();
    }

    public void Destroyy()
    {
        float destroyTime=Random.Range(minTime, maxTime);

        LeanTween.scale(gameObject, Vector3.zero, shrinkTime).setDelay(destroyTime).setEaseInOutSine();

        Destroy(gameObject, destroyTime+shrinkTime);
    }

    public void DestroyNoAnim()
    {
        Destroy(gameObject);
    }
}
