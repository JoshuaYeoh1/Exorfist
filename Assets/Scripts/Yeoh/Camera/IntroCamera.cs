using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IntroCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    public TweenAnim exorfistTitle;

    void Awake()
    {
        vcam=GetComponent<CinemachineVirtualCamera>();
    }

    void Start()
    {
        GameEventSystem.Current.OnIntroCamStart(gameObject);

        CameraManager.Current.ChangeCamera(vcam);
    }

    void RevealTitle()
    {
        exorfistTitle.TweenIn(.5f);
    }

    void IntroEndAnim()
    {
        CameraManager.Current.ChangeCameraToDefault();

        exorfistTitle.TweenOut(.5f);

        Invoke("WaitTransition", 1.5f);
    }

    void WaitTransition() // Invoked
    {
        GameEventSystem.Current.OnIntroCamEnd(gameObject);
    }
}
