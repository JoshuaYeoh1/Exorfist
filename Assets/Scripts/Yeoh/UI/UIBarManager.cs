using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    public GameObject owner;

    UIHider hider;

    void Awake()
    {
        hider=GetComponent<UIHider>();
    }

    void OnEnable()
    {
        GameEventSystem.Current.ValueBarUpdateEvent += OnValueBarUpdate;
    }
    void OnDisable()
    {
        GameEventSystem.Current.ValueBarUpdateEvent -= OnValueBarUpdate;

        LeanTween.cancel(gameObject);
    }
    
    void OnValueBarUpdate(GameObject owner, float value, float valueMax)
    {
        if(owner!=this.owner) return;

        TweenBar(value/valueMax, .2f);

        if(hider)
        {
            hider.value = value;
            hider.valueMax = valueMax;
        }
    }

    public Image bar; 

    int tweenBarId=0;

    public void TweenBar(float to, float time)
    {
        if(!bar) return;

        LeanTween.cancel(tweenBarId);

        tweenBarId = LeanTween.value(bar.fillAmount, to, time)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true)
            .setOnUpdate( (float value)=>{if(bar) bar.fillAmount=value;} )
            .id;
    }
}
