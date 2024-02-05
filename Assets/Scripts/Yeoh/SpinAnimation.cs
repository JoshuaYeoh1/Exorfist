using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnimation : MonoBehaviour
{
    public float animTime=2;
    public Vector3 spinAxis=Vector3.up;

    void Awake()
    {
        spin();
    }

    void spin()
    {
        LeanTween.rotateAround(gameObject, spinAxis, 360, animTime).setLoopClamp();
    }
}
