using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
    Image vignette;

    void Awake()
    {
        vignette=GetComponent<Image>();
    }

    public Color vignetteColor;
    float alpha;

    void Update()
    {
        vignetteColor.a = alpha;
        vignette.color = vignetteColor;
    }

    int tweenAlphaLt=0;
    public void TweenAlpha(float to, float time)
    {
        if(to<0) to=0;

        LeanTween.cancel(tweenAlphaLt);

        if(time>0)
        {
            tweenAlphaLt = LeanTween.value(alpha, to, time)
                .setEaseInOutSine()
                .setOnUpdate( (float value)=>{alpha=value;} )
                .setOnComplete(CheckResetPriority)
                .id;
        }
        else
        {
            alpha=0;
            CheckResetPriority();
        } 
    }

    float currentPriority;

    void CheckResetPriority()
    {
        if(alpha==0) currentPriority=0;
    }

    public void FlashVignette(Color color, float inTime=.01f, float wait=0, float outTime=.5f)
    {
        if(flashingRt!=null) StopCoroutine(flashingRt);
        flashingRt=StartCoroutine(Flashing(color, inTime, wait, outTime));
    }
    Coroutine flashingRt;
    IEnumerator Flashing(Color color, float inTime, float wait, float outTime)
    {
        vignetteColor=color;
        TweenAlpha(1, inTime);
        if(inTime>0) yield return new WaitForSeconds(inTime);
        if(wait>0) yield return new WaitForSeconds(wait);
        TweenAlpha(0, outTime);
    }

    public void DoVignette(Color color, float to, float time)
    {
        if(flashingRt!=null) StopCoroutine(flashingRt);

        vignetteColor=color;
        TweenAlpha(to, time);
    }

    void OnEnable()
    {
        GameEventSystem.Current.HurtEvent += OnHurt;
        GameEventSystem.Current.BlockEvent += OnBlock;
        GameEventSystem.Current.ParryEvent += OnParry;
        GameEventSystem.Current.DeathEvent += OnDeath;
        GameEventSystem.Current.AbilitySlowMoEvent += OnAbilitySlowMo;
        GameEventSystem.Current.AbilityCastEvent += OnAbilityCast;
        GameEventSystem.Current.AbilityEndEvent += OnAbilityEnd;
    }
    void OnDisable()
    {
        GameEventSystem.Current.HurtEvent -= OnHurt;
        GameEventSystem.Current.BlockEvent -= OnBlock;
        GameEventSystem.Current.ParryEvent -= OnParry;
        GameEventSystem.Current.DeathEvent -= OnDeath;
        GameEventSystem.Current.AbilitySlowMoEvent -= OnAbilitySlowMo;
        GameEventSystem.Current.AbilityCastEvent -= OnAbilityCast;
        GameEventSystem.Current.AbilityEndEvent -= OnAbilityEnd;
    }

    void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    {
        if(!hasPriority(2)) return;

        if(victim.tag=="Player")
        {
            FlashVignette(Color.red);
        }
    }

    void OnBlock(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(!hasPriority(2)) return;

        if(defender.tag=="Player")
        {
            FlashVignette(Color.cyan);
        }
    }

    void OnParry(GameObject defender, GameObject attacker, HurtInfo hurtInfo)
    {
        if(!hasPriority(2)) return;

        if(defender.tag=="Player")
        {
            FlashVignette(Color.green);
        }
    }

    void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    {
        if(!hasPriority(10)) return;

        if(victim.tag=="Player")
        {
            DoVignette(Color.red, 1, .1f);
        }
    }

    void OnAbilitySlowMo(bool toggle)
    {
        if(!hasPriority(1)) return;

        if(toggle)
        {
            DoVignette(Color.blue, 1, .5f);
        }
        else
        {
            DoVignette(Color.blue, 0, .5f);
        }
    }

    void OnAbilityCast(GameObject caster, string abilityName)
    {
        if(!hasPriority(5)) return;

        if(caster.tag=="Player")
        {
            if(abilityName=="AOE")
            {
                FlashVignette(Color.yellow);
            }
            else
            {
                DoVignette(Color.yellow, 1, .01f);
            }
        }
    }

    void OnAbilityEnd(GameObject caster, string abilityName)
    {
        if(!hasPriority(99)) return;

        if(caster.tag=="Player")
        {
            DoVignette(Color.yellow, 0, .5f);
        }
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
