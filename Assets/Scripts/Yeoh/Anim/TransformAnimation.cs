using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnimation : MonoBehaviour
{
    [Header("Rotation")]
    public bool animateRotation;
    public Vector3 rotateAngles;
    public float rotateSpeed;

    void Update()
    {
        RotationAnim();
    }

    void RotationAnim()
    {
        transform.localEulerAngles += rotateAngles * rotateSpeed * Time.deltaTime;
    }

    //public bool rotatePingPong;

    // void Start()
    // {
    //     RotationAnim();
    // }

    // void RotationAnim()
    // {
    //     if(animateRotation)
    //     {
    //         if(rotatePingPong) LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopPingPong().setEaseInOutSine();
    //         else LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopClamp();
    //     }
    // }
}
