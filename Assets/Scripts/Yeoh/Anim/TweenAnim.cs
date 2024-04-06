using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnim : MonoBehaviour
{
    [Header("Local Position")]
    public bool animPos;
    public Vector3 inPos;
    public Vector3 outPos;
    Vector3 defPos;

    [Header("Rotation")]
    public bool animRot;
    public Vector3 inRot;
    public Vector3 outRot;
    Vector3 defRot;

    [Header("Scale")]
    public bool animScale;
    public Vector3 inScale;
    public Vector3 outScale;
    Vector3 defScale;

    [Header("Autoplay")]
    public bool playOnEnable;
    public float playOnEnableAnimTime=.5f;
    public float playOnEnableDelay;

    void Awake()
    {
        defPos = transform.localPosition;
        defRot = transform.eulerAngles;
        defScale = transform.localScale;
    }

    void OnEnable()
    {
        if(playOnEnable)
        {
            if(enablingRt!=null) StopCoroutine(enablingRt);
            enablingRt = StartCoroutine(Enabling());
        }
    }
    void OnDisable()
    {
        Reset();
    }

    Coroutine enablingRt;
    IEnumerator Enabling()
    {
        yield return new WaitForSecondsRealtime(.05f);

        if(playOnEnableDelay>0)
        {
            Reset();

            yield return new WaitForSecondsRealtime(playOnEnableDelay);

            TweenIn(playOnEnableAnimTime);
        }
        else TweenIn(playOnEnableAnimTime);
    }

   void Start()
    {
        if(!playOnEnable) Reset(); // Must put after awake otherwise buttonanim records the zeroed transforms (starting transforms) as default, disappearing in mobile
    }

    public void Reset()
    {
        if(animPos) transform.localPosition = inPos;
        if(animRot) transform.eulerAngles = inRot;
        if(animScale) transform.localScale = inScale;
    }

    public void TweenIn(float time)
    {
        Reset();

        if(time>0)
        {
            LeanTween.cancel(gameObject);

            if(animPos) LeanTween.moveLocal(gameObject, defPos, time).setEaseOutExpo().setIgnoreTimeScale(true);
            if(animRot) LeanTween.rotate(gameObject, defRot, time).setEaseInOutSine().setIgnoreTimeScale(true);
            if(animScale) LeanTween.scale(gameObject, defScale, time).setEaseOutCubic().setIgnoreTimeScale(true);

            AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICooldown, transform.position, false);
        }
        else
        {
            if(animPos) transform.localPosition = defPos;
            if(animRot) transform.eulerAngles = defRot;
            if(animScale) transform.localScale = defScale;
        }
    }

    public void TweenOut(float time)
    {
        TweenIn(0);

        if(time>0)
        {
            LeanTween.cancel(gameObject);

            if(animPos) LeanTween.moveLocal(gameObject, outPos, time).setEaseInExpo().setIgnoreTimeScale(true).setOnComplete(Reset);
            if(animRot) LeanTween.rotate(gameObject, outRot, time).setEaseInOutSine().setIgnoreTimeScale(true).setOnComplete(Reset);
            if(animScale) LeanTween.scale(gameObject, outScale, time).setEaseInCubic().setIgnoreTimeScale(true).setOnComplete(Reset);

            AudioManager.Current.PlaySFX(SFXManager.Current.sfxUICooldown, transform.position, false);
        }
        else
        {
            if(animPos) transform.localPosition = outPos;
            if(animRot) transform.eulerAngles = outRot;
            if(animScale) transform.localScale = outScale;
        }
    }

    //[Button] // requires Odin Inspector??
    [ContextMenu("Record Local Position")]
    void RecordCurrentPosition()
    {
        inPos=transform.localPosition;
        outPos=transform.localPosition;
    }
    [ContextMenu("Record Rotation")]
    void RecordCurrentRotation()
    {
        inRot=transform.eulerAngles;
        outRot=transform.eulerAngles;
    }
    [ContextMenu("Record Scale")]
    void RecordCurrentScale()
    {
        inScale=transform.localScale;
        outScale=transform.localScale;
    }
    
}
