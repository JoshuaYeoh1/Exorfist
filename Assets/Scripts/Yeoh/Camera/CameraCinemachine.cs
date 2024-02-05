using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraCinemachine : MonoBehaviour
{
    [HideInInspector] public CinemachineFreeLook cm;
    [HideInInspector] public float defaultSize, currentSize;
    CinemachineBasicMultiChannelPerlin[] cbmcp;
    float defaultAmplitude, defaultFrequency;
    public float shakeAmplitude=2, shakeFrequency=2;

    void Awake()
    {
        cm=GetComponent<CinemachineFreeLook>();

        cbmcp = cm.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();
        defaultAmplitude = cbmcp[0].m_AmplitudeGain;
        defaultFrequency = cbmcp[0].m_FrequencyGain;

        defaultSize=currentSize=cm.m_Lens.OrthographicSize;
    }

    void FixedUpdate()
    {
        if(currentSize!=cm.m_Lens.OrthographicSize) currentSize=cm.m_Lens.OrthographicSize;
    }

    // int camMoveLt=0;
    // public void moveCam(Vector3 newPos, float time)
    // {
    //     cancelMoveCam();
    //     camMoveLt = LeanTween.move(follow.gameObject, newPos, time).setEaseInOutSine().id;
    // }
    // public void cancelMoveCam()
    // {
    //     LeanTween.cancel(camMoveLt);
    // }

    int camSizeLt=0;
    public void changeCamSize(float newCamSize, float time)
    {
        cancelCamSize();
        camSizeLt = LeanTween.value(cm.m_Lens.OrthographicSize, newCamSize, time).setEaseInOutSine().setOnUpdate(TweenUpdate).id;

        //Singleton.instance.playSFX(Singleton.instance.sfxCamPan, transform, false);
    }
    void TweenUpdate(float value)
    {
        cm.m_Lens.OrthographicSize = value;
    }
    public void cancelCamSize()
    {
        LeanTween.cancel(camSizeLt);
    }

    public void doShake(bool toggle=true)
    {
        foreach(CinemachineBasicMultiChannelPerlin _cbmcp in cbmcp)
        {
            if(toggle)
            {
                _cbmcp.m_AmplitudeGain = shakeAmplitude;
                _cbmcp.m_FrequencyGain = shakeFrequency;
            }
            else
            {
                _cbmcp.m_AmplitudeGain = defaultAmplitude;
                _cbmcp.m_FrequencyGain = defaultFrequency;
            }
        }
    }
    
    Coroutine shakeRt;
    public void shake(float time)
    {
        if(shakeRt!=null) StopCoroutine(shakeRt);
        shakeRt=StartCoroutine(shaking(time));
    }
    IEnumerator shaking(float time)
    {
        doShake(true);
        yield return new WaitForSecondsRealtime(time);
        doShake(false);
    }
}
