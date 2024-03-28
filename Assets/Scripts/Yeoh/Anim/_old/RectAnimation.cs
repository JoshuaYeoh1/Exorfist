// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class RectAnimation : MonoBehaviour
// {
//     RectTransform rect;

//     [Header("Rotation")]
//     public bool animateRotation;
//     public Vector3 rotateAngles;
//     public float rotateSpeed;

//     [Header("Time")]
//     public bool ignoreTime;
//     float deltaTime;

//     void Awake()
//     {
//         rect = GetComponent<RectTransform>();
//     }

//     void Update()
//     {
//         if(ignoreTime)
//         {
//             deltaTime=Time.unscaledDeltaTime;
//         }
//         else
//         {
//             deltaTime=Time.deltaTime;
//         }

//         RotationAnim();
//     }

//     void RotationAnim()
//     {
//         rect.localEulerAngles += rotateAngles * rotateSpeed * deltaTime;
//     }

//     //public bool rotatePingPong;

//     // void Start()
//     // {
//     //     RotationAnim();
//     // }

//     // void RotationAnim()
//     // {
//     //     if(animateRotation)
//     //     {
//     //         if(rotatePingPong) LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopPingPong().setEaseInOutSine();
//     //         else LeanTween.rotateLocal(gameObject, rotateAngles, rotateTime).setLoopClamp();
//     //     }
//     // }
// }
