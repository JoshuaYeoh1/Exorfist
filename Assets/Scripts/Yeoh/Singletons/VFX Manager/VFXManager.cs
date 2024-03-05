using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VFXManager : MonoBehaviour
{
    public static VFXManager current;

    void Awake()
    {
        if(!current) current=this;
        else Destroy(gameObject);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void CamShake(float time=.1f, float amp=1.5f, float freq=2)
    {
        GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<CameraCinemachine>().Shake(time, amp, freq);

        Vibrator.Vibrate();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int tweenTimeLt=0;
    public void TweenTime(float to, float time=.01f)
    {
        LeanTween.cancel(tweenTimeLt);
        tweenTimeLt = LeanTween.value(Time.timeScale, to, time)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setOnUpdate( (float value)=>{Time.timeScale=value;} )
            .id;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HideInInspector] public bool canHitStop=true;

    public void HitStop(float fadeIn=.01f, float wait=.05f, float fadeOut=.25f)
    {
        if(canHitStop)
        {
            if(hitStoppingRt!=null) StopCoroutine(hitStoppingRt);
            hitStoppingRt = StartCoroutine(HitStopping(fadeIn, wait, fadeOut));
        }
    }
    Coroutine hitStoppingRt;
    IEnumerator HitStopping(float fadeIn, float wait, float fadeOut)
    {
        TweenTime(0, fadeIn);
        yield return new WaitForSecondsRealtime(fadeIn + wait);
        TweenTime(1, fadeOut);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public GameObject popUpTextPrefab;

    public void SpawnPopUpText(Vector3 pos, string text, Color color, float scaleMult=.35f, float force=2f)
    {
        GameObject popUp = Instantiate(popUpTextPrefab, pos, Quaternion.identity);
        popUp.hideFlags = HideFlags.HideInHierarchy;

        popUp.transform.localScale *= scaleMult;

        Rigidbody rb = popUp.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up*force, ForceMode.Impulse);

        TextMeshProUGUI[] tmps = popUp.GetComponentsInChildren<TextMeshProUGUI>();

        foreach(TextMeshProUGUI tmp in tmps)
        {
            tmp.text = text;
            tmp.color = color;
        }
    }
}
