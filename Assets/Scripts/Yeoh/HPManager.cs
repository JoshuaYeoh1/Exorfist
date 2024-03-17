using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public float hp=100;
    [HideInInspector] public float hpMax;

    [Header("Regeneration")]
    public bool regen;
    public bool regenWhenEmpty;
    public float regenHp=.2f, regenInterval=.1f;
    [HideInInspector] public float defaultRegenHp;

    [Header("UI Bar")]
    public GameObject hpBarFill;
    public InOutAnim hpBar;
    public bool hideWhenFull=true, hideWhenEmpty;
    public float animTime=.5f;

    void Awake()
    {
        hpMax=hp;

        defaultRegenHp=regenHp;
    } 
    
    void OnEnable()
    {
        StartCoroutine(HpRegenerating());
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);

        CheckUIBarVisibility();
    }

    bool canShow=true, canHide;

    void CheckUIBarVisibility()
    {
        if(hpBar)
        {
            if(canHide && ((hideWhenFull && hp>=hpMax) || (hideWhenEmpty && hp<=0)) )
            {
                hpBar.animOut(animTime);

                canHide=false;
                Invoke("ToggleShow", animTime);                
            }

            if(canShow && hp>0 && hp<hpMax)
            {
                hpBar.animIn(animTime);

                canShow=false;
                Invoke("ToggleHide", animTime);
            }
        }
    }

    void ToggleHide()
    {
        canHide=!canHide;
    }
    void ToggleShow()
    {
        canShow=!canShow;
    }

    public void Hit(float dmg)
    {
        if(dmg>0)
        {
            if(hp>dmg) hp-=dmg;
            else hp=0;
        }
        
        UpdateHpBar();
    }    
    
    IEnumerator HpRegenerating()
    {
        while(true)
        {
            yield return new WaitForSeconds(regenInterval);

            if(hp<hpMax && (hp>0 || regenWhenEmpty) )
            {
                if(regen) Add(regenHp);

                UpdateHpBar();
            }
        }
    }

    public void Add(float amount)
    {
        if(amount < hpMax-hp) hp+=amount;
        else hp=hpMax;
    }

    int hpBarLt=0;

    public void UpdateHpBar()
    {
        if(hpBarFill)
        {
            LeanTween.cancel(hpBarLt);
            hpBarLt = LeanTween.scaleX(hpBarFill, hp/hpMax, .2f).setEaseOutSine().id;
        }
    }
}
