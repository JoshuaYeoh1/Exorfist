using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnim : MonoBehaviour
{
    Image img;
    SpriteRenderer sr;
    Vector2 defscale;
    Sprite defSprite;

    public Sprite hoverSprite;
    public float animTime=.3f, scaleMult=.1f;
    bool inBtn, pressedBtn;

    void Awake()
    {
        img = GetComponent<Image>();
        sr = GetComponent<SpriteRenderer>();
        defscale = transform.localScale;

        ToggleHoverSprite(false);
    }

    void OnDisable()
    {
        LeanTween.cancel(gameObject);
        ResetButton();
        ToggleHoverSprite(false);
        inBtn=pressedBtn=false;
    }

    public void OnMouseEnter()
    {
        inBtn=true;

        if(!pressedBtn)
        {
            ToggleHoverSprite(true);

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defscale*(1+scaleMult), animTime).setEaseOutExpo().setIgnoreTimeScale(true);
        }
        //Singleton.instance.playSFX(Singleton.instance.sfxBtnHover,transform,false);
    }

    public void OnMouseExit()
    {
        inBtn=false;

        if(!pressedBtn)
        {
            ToggleHoverSprite(false);

            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, defscale, animTime).setEaseOutExpo().setIgnoreTimeScale(true);
        }
    }

    public void OnMouseDown()
    {
        if(inBtn)
        {
            pressedBtn=true;

            LeanTween.cancel(gameObject);
            transform.localScale = defscale*(1+scaleMult);
            LeanTween.scale(gameObject, defscale*(1-scaleMult), animTime/2).setEaseOutExpo().setIgnoreTimeScale(true);
        }
        //Singleton.instance.playSFX(Singleton.instance.sfxBtnClick,transform,false);
    }

    public void OnMouseUp()
    {
        pressedBtn=false;

        LeanTween.cancel(gameObject);

        if(inBtn)
        LeanTween.scale(gameObject, defscale*(1+scaleMult), animTime/2).setDelay(animTime/2).setEaseOutBack().setIgnoreTimeScale(true);

        else
        LeanTween.scale(gameObject, defscale, animTime).setEaseOutBack().setIgnoreTimeScale(true);
    }

    public void ToggleHoverSprite(bool toggle)
    {
        if(hoverSprite)
        {
            if(sr)
            {
                if(toggle) sr.sprite=hoverSprite;
                else sr.sprite=defSprite;
            }
            else if(img)
            {
                if(toggle) img.sprite=hoverSprite;
                else img.sprite=defSprite;
            }
        }
    }

    public void ResetButton()
    {
        transform.localScale = defscale;
    }
}
