using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    public float animTime=.3f, scaleMult=.1f;
    bool inBtn, pressedBtn;

    Vector2 defScale;

    void Awake()
    {
        defScale = transform.localScale;
    }

    void OnDisable()
    {
        LeanTween.cancel(gameObject);
        ResetButton();
        inBtn=pressedBtn=false;
    }

    public void OnMouseEnter()
    {
        inBtn=true;

        if(!pressedBtn)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defScale*(1+scaleMult), animTime).setEaseOutExpo().setIgnoreTimeScale(true);
        }
        //Singleton.instance.playSFX(Singleton.instance.sfxBtnHover,transform,false);
    }

    public void OnMouseExit()
    {
        inBtn=false;

        if(!pressedBtn)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defScale, animTime).setEaseOutExpo().setIgnoreTimeScale(true);
        }
    }

    public void OnMouseDown()
    {
        if(inBtn)
        {
            pressedBtn=true;

            LeanTween.cancel(gameObject);
            transform.localScale = defScale*(1+scaleMult);
            LeanTween.scale(gameObject, defScale*(1-scaleMult), animTime/2).setEaseOutExpo().setIgnoreTimeScale(true);
        }
        //Singleton.instance.playSFX(Singleton.instance.sfxBtnClick,transform,false);
    }

    public void OnMouseUp()
    {
        pressedBtn=false;

        LeanTween.cancel(gameObject);

        if(inBtn)
        LeanTween.scale(gameObject, defScale*(1+scaleMult), animTime/2).setDelay(animTime/2).setEaseOutBack().setIgnoreTimeScale(true);

        else
        LeanTween.scale(gameObject, defScale, animTime).setEaseOutBack().setIgnoreTimeScale(true);
    }

    public void ResetButton()
    {
        transform.localScale = defScale;
    }
}
