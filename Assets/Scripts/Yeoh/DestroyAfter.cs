using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float destroyTimeMin=3, destroyTimeMax=4, animTime=.5f;

    void Start()
    {
        float destroyTime=Random.Range(destroyTimeMin, destroyTimeMax);

        LeanTween.scale(gameObject, Vector3.zero, animTime).setDelay(destroyTime).setEaseInOutSine();

        Destroy(gameObject, destroyTime+animTime);
    }
}
