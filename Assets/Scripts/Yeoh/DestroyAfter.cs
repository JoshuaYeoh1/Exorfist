using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool destroyOnStart;
    public float minTime=3, maxTime=4;

    [Header("Shrink Anim")]
    public List<GameObject> shrinkObjects = new List<GameObject>();
    public float shrinkTime=.5f;

    void Start()
    {
        if(destroyOnStart) Destroyy();
    }

    public void Destroyy()
    {
        float waitTime=Random.Range(minTime, maxTime);

        ShrinkAnim(waitTime);

        Destroy(gameObject, waitTime+shrinkTime);
    }

    public void ShrinkAnim(float waitTime)
    {
        List<GameObject> objectsToShrink = new List<GameObject>();

        if(shrinkObjects.Count>0) objectsToShrink = shrinkObjects;
        else objectsToShrink.Add(gameObject);

        foreach(GameObject obj in objectsToShrink)
        {
            LeanTween.scale(obj, Vector3.zero, shrinkTime).setDelay(waitTime).setEaseInOutSine();
        }
    }

    public void DestroyNoAnim()
    {
        Destroy(gameObject);
    }
}
