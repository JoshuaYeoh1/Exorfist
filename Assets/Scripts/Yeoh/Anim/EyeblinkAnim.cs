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

    List<int> tweenEyesLts = new List<int>();

    public void TweenEyesY(float to, float time)
    {
        foreach(int id in tweenEyesLts)
        {
            LeanTween.cancel(id);
        }

        tweenEyesLts.Clear();

        foreach(GameObject eye in eyes)
        {
            int id = LeanTween.scaleY(eye, to, time).setEaseInOutSine().id;

            tweenEyesLts.Add(id);
        }
    }

    public float blinkInterval=1;

    Coroutine randomBlinkingRt;
    IEnumerator RandomBlinking()
    {
        while(true)
        {
            yield return new WaitForSeconds(blinkInterval * Random.Range(.5f, 3));
            Blink();
        }
    }

    public float blinkTime=.2f;

    public void Blink()
    {
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
        GameEventSystem.Current.BlockEvent += OnBlock;
        GameEventSystem.Current.DeathEvent += OnDeath;

        if(!isDead) randomBlinkingRt = StartCoroutine(RandomBlinking());
    }

    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.BlockEvent -= OnBlock;
        GameEventSystem.Current.DeathEvent -= OnDeath;
    }

    public void OnHurt(GameObject victim, GameObject attacker, float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(victim!=owner) return;

        if(!isDead) Blink();
    }

    public void OnBlock(GameObject defender, GameObject attacker, Vector3 contactPoint, bool parry, bool broke)
    {
        if(defender!=owner) return;

        if(!isDead) Blink();
    }

    bool isDead;

    public void OnDeath(GameObject victim, GameObject killer, float dmg, float kbForce, Vector3 contactPoint)
    {
        if(victim!=owner) return;

        isDead=true;

        if(randomBlinkingRt!=null) StopCoroutine(randomBlinkingRt);
        if(blinkingRt!=null) StopCoroutine(blinkingRt);

        TweenEyesY(.2f, blinkTime);
    }
}
