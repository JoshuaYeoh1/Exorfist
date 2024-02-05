using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public bool regen;
    public float hp=100, hpMax=100, regenHp=.2f, regenTime=.1f;
    public GameObject hpBarFill;
    public InOutAnim hpBar;
    bool canShow=true, canHide;

    void Awake()
    {
        hp=hpMax;
        StartCoroutine(HpRegen());
    } 

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);

        if(hpBar)
        {
            if(hp>0 && hp<hpMax && canShow)
            {
                canShow=false;

                hpBar.animIn(.5f);

                Invoke("ToggleHide",.5f);
            }
            else if((hp<=0 || hp>=hpMax) && canHide)
            {
                canHide=false;

                hpBar.animOut(.5f);

                Invoke("ToggleShow",.5f);
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
        if(hp>dmg) hp-=dmg;
        else hp=0;
        UpdateHpBar();
    }    
    
    IEnumerator HpRegen()
    {
        while(true)
        {
            yield return new WaitForSeconds(regenTime);

            if(regen && hp>0 && hp<hpMax)
            {   
                if(hp<=hpMax-regenHp) hp+=regenHp;
                else hp=hpMax;

                UpdateHpBar();
            }
        }
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
