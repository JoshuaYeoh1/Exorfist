using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeblinkAnim : MonoBehaviour
{
    public GameObject owner;

    public List<GameObject> eyes = new List<GameObject>();

    float defScaleY;

    void Start()
    {
        defScaleY = eyes[0].transform.localScale.y;
    }

    List<int> eyeTweenIDs = new List<int>();

    public void TweenEyesY(float to, float time)
    {
        if(to<0) to=0;

        foreach(int id in eyeTweenIDs)
        {
            LeanTween.cancel(id);
            eyeTweenIDs.Remove(id);
        }

        foreach(GameObject eye in eyes)
        {
            if(time>0) 
            {
                int id = LeanTween.scaleY(eye, to, time).setEaseInOutSine().setOnComplete(ResetPriority).id;

                eyeTweenIDs.Add(id);
            }
            else
            {
                eye.transform.localScale = new Vector3(eye.transform.localScale.x, to, eye.transform.localScale.z);

                ResetPriority();
            }
        }
    }

    float currentPriority;

    void ResetPriority()
    {
        currentPriority=0;
    }

    public float blinkInterval=1;

    Coroutine randomBlinkingRt;
    IEnumerator RandomBlinking()
    {
        while(true)
        {
            yield return new WaitForSeconds(blinkInterval * Random.Range(.5f, 3));
            if(hasPriority(1)) Blink();
        }
    }

    public float blinkTime=.2f;

    public void Blink()
    {
        if(isDead) return;

        if(blinkingRt!=null) StopCoroutine(blinkingRt);
        StartCoroutine(Blinking());
    }

    Coroutine blinkingRt;
    IEnumerator Blinking()
    {
        TweenEyesY(0, blinkTime);
        yield return new WaitForSeconds(blinkTime);
        TweenEyesY(defScaleY, blinkTime);
    }

    void OnEnable()
    {
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.DeathEvent += OnDeath;

        if(!isDead) randomBlinkingRt = StartCoroutine(RandomBlinking());
    }

    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.DeathEvent -= OnDeath;
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(!hasPriority(2)) return;

        if(victim!=owner) return;

        Blink();
    }

    bool isDead;

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(!hasPriority(5)) return;

        if(victim!=owner) return;

        isDead=true;

        if(randomBlinkingRt!=null) StopCoroutine(randomBlinkingRt);
        if(blinkingRt!=null) StopCoroutine(blinkingRt);

        TweenEyesY(.2f, blinkTime);
    }

    bool hasPriority(float level)
    {
        if(level>=currentPriority)
        {
            currentPriority = level;
            return true;
        }
        else return false;
    }
}
