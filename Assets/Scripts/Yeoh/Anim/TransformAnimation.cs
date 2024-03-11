using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnimation : MonoBehaviour
{
    [Header("Rotation")]
    public bool animateRotation;
    public Vector3 rotateAngles; // doesnt work if you put 360 or nearer
    public float rotateTime;
    public bool rotatePingPong;

    void Start()
    {
        RotationAnim();
    }

    void RotationAnim()
    {
        if(animateRotation)
        {
            if(rotatePingPong) LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopPingPong().setEaseInOutSine();
            else LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopClamp();
        }
    }
}
