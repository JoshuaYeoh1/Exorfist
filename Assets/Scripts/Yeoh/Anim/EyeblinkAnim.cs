using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeblinkAnim : MonoBehaviour
{
    public GameObject owner;

    public List<GameObject> eyes = new List<GameObject>();

    float defScaleY;

    void Awake()
    {
        defScaleY = eyes[0].transform.localScale.y;
    }

    Dictionary<GameObject, int> eyeTweenIdDict = new Dictionary<GameObject, int>();

    public void TweenEyesY(float to, float time)
    {
        if(to<0) to=0;

        foreach(GameObject eye in eyes)
        {
            if(time>0) 
            {
                if(eyeTweenIdDict.ContainsKey(eye))
                {
                    LeanTween.cancel(eyeTweenIdDict[eye]);
                    eyeTweenIdDict.Remove(eye); // cleanup
                }

                int id = LeanTween.scaleY(eye, to, time).setEaseInOutSine().id; //.setOnComplete(CheckResetPriority)

                eyeTweenIdDict.Add(eye, id);
            }
            else
            {
                eye.transform.localScale = new Vector3(eye.transform.localScale.x, to, eye.transform.localScale.z);

                //CheckResetPriority();
            }
        }

        if(to==defScaleY) canBlink=true;
        else canBlink=false;
    }

    // float currentPriority;

    // void CheckResetPriority()
    // {
    //     foreach(GameObject eye in eyes)
    //     {
    //         if(eye.transform.localScale.y==defScaleY)
    //         {
    //             currentPriority=0;
    //         }
    //     }
    // }

    bool canBlink=true;
    public float blinkTime=.2f;

    public void Blink()
    {
        if(canBlink)
        {
            CancelBlink();
            blinkingRt = StartCoroutine(Blinking());
        }
    }

    Coroutine blinkingRt;
    IEnumerator Blinking()
    {
        TweenEyesY(0, blinkTime);
        yield return new WaitForSeconds(blinkTime);
        TweenEyesY(defScaleY, blinkTime);
    }

    void CancelBlink()
    {
        if(blinkingRt!=null) StopCoroutine(blinkingRt);
        //CheckResetPriority();
    }

    void OnEnable()
    {
        StartRandomBlinking();
    }

    public float blinkInterval=1;

    void StartRandomBlinking()
    {
        if(canBlink) randomBlinkingRt = StartCoroutine(RandomBlinking());
    }

    Coroutine randomBlinkingRt;
    IEnumerator RandomBlinking()
    {
        while(true)
        {
            yield return new WaitForSeconds(blinkInterval * Random.Range(.5f, 3));
            Blink();
            //if(hasPriority(1)) 
        }
    }

    void StopRandomBlinking()
    {
        if(randomBlinkingRt!=null) StopCoroutine(randomBlinkingRt);
    }

    void Start()
    {
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.RespawnEvent += OnRespawn;
    }
    void OnDestroy()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.RespawnEvent -= OnRespawn;
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        //if(!hasPriority(2)) return;

        if(victim==owner)
        {
            Blink();
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        //if(!hasPriority(5)) return;

        if(victim==owner)
        {
            canBlink=false;

            StopRandomBlinking();
            CancelBlink();

            TweenEyesY(.2f, blinkTime);
        }
    }

    void OnRespawn(GameObject zombo)
    {
        if(zombo.tag!="Player") return;

        TweenEyesY(1, blinkTime);

        canBlink=true;
        StartRandomBlinking();
    }

    // bool hasPriority(float level)
    // {
    //     if(level>=currentPriority)
    //     {
    //         currentPriority = level;
    //         return true;
    //     }
    //     else return false;
    // }
}
