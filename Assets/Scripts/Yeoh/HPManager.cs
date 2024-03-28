using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public float hp=100;
    public float hpMax=100;

    [Header("Regeneration")]
    public bool regen;
    public bool regenWhenEmpty;
    public float regenHp=.2f, regenInterval=.1f;
    [HideInInspector] public float defaultRegenHp;

    void Awake()
    {
        defaultRegenHp=regenHp;
        
        GameEventSystem.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    void Update()
    {
        hp = Mathf.Clamp(hp, 0, hpMax);        
    }
    
    void OnEnable()
    {
        StartCoroutine(HpRegenerating());
    }

    public void Hit(float dmg)
    {
        if(dmg>0)
        {
            if(hp>dmg) hp-=dmg;
            else hp=0;
        }
        
        GameEventSystem.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }    
    
    IEnumerator HpRegenerating()
    {
        while(true)
        {
            yield return new WaitForSeconds(regenInterval);

            if(hp<hpMax && (hp>0 || regenWhenEmpty) )
            {
                if(regen) Add(regenHp);

                GameEventSystem.Current.OnUIBarUpdate(gameObject, hp, hpMax);
            }
        }
    }

    public void Add(float amount)
    {
        if(amount < hpMax-hp) hp+=amount;
        else hp=hpMax;

        GameEventSystem.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }

    public void SetHPPercent(float percent)
    {
        if(percent==0 || percent>100) return;

        hp = hpMax * percent/100;

        GameEventSystem.Current.OnUIBarUpdate(gameObject, hp, hpMax);
    }
}
