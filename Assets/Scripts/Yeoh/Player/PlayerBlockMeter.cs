using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockMeter : MonoBehaviour
{
    HPManager hp;

    public GameObject player;
    PlayerBlock block;
    PlayerHurt hurt;

    [HideInInspector] public bool regen;
    public float regenCooldown=3;
    public float blockBreakSpeedDebuffMult=.5f, blockBreakPenaltyStunTime=1;

    void Awake()
    {
        hp = GetComponent<HPManager>();

        block = player.GetComponent<PlayerBlock>();
        hurt = player.GetComponent<PlayerHurt>();
    }

    void Update()
    {
        if(hp.regen!=regen) hp.regen=regen;
    }

    Coroutine regenCoolingRt;
    public void RedoRegenCooling()
    {
        CancelRegenCooling();
        regenCoolingRt = StartCoroutine(regenCooling());
    }
    IEnumerator regenCooling()
    {
        yield return new WaitForSeconds(regenCooldown);
        regen=true;
    }
    public void CancelRegenCooling()
    {
        regen=false;
        if(regenCoolingRt!=null) StopCoroutine(regenCoolingRt);
    }

    public bool IsEmpty()
    {
        if(hp.hp<=0) return true;
        return false;
    }

    public void Refill(float percent)
    {
        hp.Add(hp.hpMax*percent/100);
    }

    public void Hurt(GameObject attacker, HurtInfo hurtInfo)
    {
        if(!hurt.iframe)
        {
            hp.Hit(hurtInfo.dmg);

            if(hp.hp>0) // if not empty yet
            {
                BlockHit(attacker, hurtInfo);
            }   
            else
            {
                BlockBreak(attacker, hurtInfo);
            }
        }        
    }

    void BlockHit(GameObject attacker, HurtInfo hurtInfo)
    {
        block.canBlock=true;

        block.player.anim.CrossFade("block hit", .1f, 3, 0);

        hurt.Knockback(hurtInfo.kbForce*block.blockKnockbackResistMult, hurtInfo.contactPoint);

        hurt.DoIFraming(hurt.iframeTime, -.5f, .5f, .5f); // flicker cyan

        hurtInfo.block=true;

        GameEventSystem.Current.OnHurt(block.gameObject, attacker, hurtInfo);

        //move to sfx manager later
        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }

    void BlockBreak(GameObject attacker, HurtInfo hurtInfo)
    {
        block.Unblock();
        
        hurtInfo.dmg *= .5f;
        hurtInfo.speedDebuffMult = blockBreakSpeedDebuffMult;
        hurtInfo.stunTime = blockBreakPenaltyStunTime;

        hurt.Hurt(attacker, hurtInfo);

        hurtInfo.blockBreak=true;

        GameEventSystem.Current.OnHurt(block.gameObject, attacker, hurtInfo);

        //move to sfx manager later
        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }
}
