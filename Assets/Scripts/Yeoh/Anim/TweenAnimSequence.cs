using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimSequence : MonoBehaviour
{
    public List<TweenAnim> animsList = new List<TweenAnim>();
    
    public float animTime=.5f;
    public float nextAnimOffsetTime=-.25f;

    [Header("Autoplay")]
    public bool playOnEnable;
    public float playOnEnableAnimTime=.5f;
    public float playOnEnableDelay;

    void OnEnable()
    {
        if(playOnEnable)
        {
            ResetAll();

            Invoke("Play", playOnEnableDelay);
        }
    }

    public void Play()
    {
        if(!playOnEnable) ResetAll();

        if(tweeningInRt!=null) StopCoroutine(tweeningInRt);
        tweeningInRt = StartCoroutine(TweeningIn());
    }

    Coroutine tweeningInRt;
    IEnumerator TweeningIn()
    {
        for(int i=0; i<animsList.Count; i++)
        {
            animsList[i].TweenIn(animTime);
            yield return new WaitForSecondsRealtime(animTime+nextAnimOffsetTime);
        }
    }

    public void Reverse()
    {
        SetInAll();

        if(tweeningOutRt!=null) StopCoroutine(tweeningOutRt);
        tweeningOutRt = StartCoroutine(TweeningOut());
    }

    Coroutine tweeningOutRt;
    IEnumerator TweeningOut()
    {
        for(int i=animsList.Count-1; i>=0; i--)
        {
            animsList[i].TweenOut(animTime);
            yield return new WaitForSecondsRealtime(animTime+nextAnimOffsetTime);
        }
    }

    void ResetAll()
    {
        foreach(TweenAnim anim in animsList)
        {
            anim.Reset();
        }
    }
    void SetInAll()
    {
        foreach(TweenAnim anim in animsList)
        {
            anim.TweenIn(0);
        }
    }
}
