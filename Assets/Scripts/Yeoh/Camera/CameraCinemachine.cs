using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraCinemachine : MonoBehaviour
{
    [HideInInspector] public CinemachineFreeLook cm;
    CinemachineBasicMultiChannelPerlin[] cbmcp;

    [HideInInspector] public float defaultSize, defaultAmplitude, defaultFrequency;

    void Awake()
    {
        cm=GetComponent<CinemachineFreeLook>();
        cbmcp = cm.GetComponentsInChildren<CinemachineBasicMultiChannelPerlin>();

        defaultSize=cm.m_Lens.OrthographicSize;
        defaultAmplitude = cbmcp[0].m_AmplitudeGain;
        defaultFrequency = cbmcp[0].m_FrequencyGain;
    }

    int camSizeLt=0;
    public void ChangeCamSize(float newCamSize, float time)
    {
        LeanTween.cancel(camSizeLt);
        camSizeLt = LeanTween.value(cm.m_Lens.OrthographicSize, newCamSize, time)
                        .setEaseInOutSine()
                        .setOnUpdate( (float value)=>{cm.m_Lens.OrthographicSize=value;} )
                        .id;

        //Singleton.instance.playSFX(Singleton.instance.sfxCamPan, transform, false);
    }
    
    Coroutine shakeRt;
    public void Shake(float time, float amp, float freq)
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

    public void DoShake(bool toggle=true, float amp=0, float freq=0)
    {
        foreach(CinemachineBasicMultiChannelPerlin _cbmcp in cbmcp)
        {
            if(toggle)
            {
                _cbmcp.m_AmplitudeGain = amp;
                _cbmcp.m_FrequencyGain = freq;
            }
            else
            {
                _cbmcp.m_AmplitudeGain = defaultAmplitude;
                _cbmcp.m_FrequencyGain = defaultFrequency;
            }
        }
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
