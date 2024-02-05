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
    public float shakeAmplitude=.5f, shakeFrequency=2;

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
    
    Coroutine shakeRt;
    public void Shake(float time, float amp=.5f, float freq=2)
    {
        if(shakeRt!=null) StopCoroutine(shakeRt);
        shakeRt=StartCoroutine(Shaking(time, amp, freq));
    }
    IEnumerator Shaking(float t, float amp, float freq)
    {
        DoShake(true, amp, freq);
        yield return new WaitForSecondsRealtime(t);
        DoShake(false);
    }

    public void DoShake(bool toggle=true, float amp=.5f, float freq=2)
    {
        float origAmp = shakeAmplitude;
        float origFreq = shakeFrequency;

        shakeAmplitude=amp;
        shakeFrequency=freq;

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

        shakeAmplitude=origAmp;
        shakeFrequency=origFreq;
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
}
