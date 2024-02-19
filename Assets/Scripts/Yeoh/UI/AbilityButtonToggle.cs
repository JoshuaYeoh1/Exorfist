using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonToggle : MonoBehaviour
{
    public GameObject group1, group2;
    public float slowMoTime=3, slowMoMult=.1f;

    public void ShowAbilities()
    {
        group1.SetActive(false);
        group2.SetActive(true);

        Singleton.instance.canHitStop=false;

        Singleton.instance.TweenTime(slowMoMult, .25f);

        slowMoCountdownRt = StartCoroutine(SlowMoCountdown());
    }

    Coroutine slowMoCountdownRt;
    IEnumerator SlowMoCountdown()
    {
        yield return new WaitForSecondsRealtime(3);
        HideAbilities();
    }

    public void HideAbilities()
    {
        if(slowMoCountdownRt!=null) StopCoroutine(slowMoCountdownRt);

        group1.SetActive(true);
        group2.SetActive(false);

        Singleton.instance.TweenTime(1);

        Singleton.instance.canHitStop=true;
    }
}
