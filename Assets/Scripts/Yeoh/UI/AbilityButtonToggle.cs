using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonToggle : MonoBehaviour
{
    public GameObject group1, group2;
    public float slowMoTime=3, slowMoMult=.5f;

    public void ShowAbilities()
    {
        group1.SetActive(false);
        group2.SetActive(true);

        // move to vfx manager later
        VFXManager.Current.canHitStop=false;

        // move to vfx manager later
        VFXManager.Current.TweenTime(slowMoMult, .5f);

        slowMoCountdownRt = StartCoroutine(SlowMoCountdown());
    }

    Coroutine slowMoCountdownRt;
    IEnumerator SlowMoCountdown()
    {
        yield return new WaitForSecondsRealtime(slowMoTime);
        HideAbilities();
    }

    public void HideAbilities()
    {
        if(slowMoCountdownRt!=null) StopCoroutine(slowMoCountdownRt);

        group1.SetActive(true);
        group2.SetActive(false);

        // move to vfx manager later
        VFXManager.Current.TweenTime(1, .5f);
        VFXManager.Current.canHitStop=true;
    }
}
