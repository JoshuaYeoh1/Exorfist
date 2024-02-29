using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockMeter : MonoBehaviour
{
    HPManager hp;
    PlayerBlock block;

    [HideInInspector] public bool iframe, regen;
    public float iframeTime=.3f, regenCooldown=3;
    public float blockBreakSpeedDebuffMult=.5f, blockBreakPenaltyStunTime=1;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        block=transform.root.GetComponent<PlayerBlock>();
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

    public void Hit(float dmg, float kbForce, Vector3 contactPoint)
    {
        if(!iframe)
        {
            DoIFraming(iframeTime);

            hp.Hit(dmg);

            if(hp.hp>0) // if not empty yet
            {
                BlockHit(dmg, kbForce, contactPoint);
            }   
            else
            {
                BlockBreak(dmg, kbForce, contactPoint);
            }
        }        
    }

    public void DoIFraming(float t)
    {
        StartCoroutine(iframing(t));
    }
    IEnumerator iframing(float t)
    {
        iframe=true;
        yield return new WaitForSeconds(t);
        iframe=false;
    }

    void BlockHit(float dmg, float kbForce, Vector3 contactPoint)
    {
        block.canBlock=true;

        block.player.anim.CrossFade("block hit", .1f, 4, 0);

        block.hurt.Knockback(kbForce*block.blockKnockbackResistMult, contactPoint);

        block.flash.SpawnFlash(contactPoint, Color.white);

        block.color.FlashColor(.1f, .5f, .5f, .5f); // flash white

        Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.cyan);

        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }

    void BlockBreak(float dmg, float kbForce, Vector3 contactPoint)
    {
        block.Unblock();
        
        block.hurt.Hit(dmg*.5f, kbForce, contactPoint, blockBreakSpeedDebuffMult, blockBreakPenaltyStunTime);

        block.flash.SpawnFlash(contactPoint, Color.red);

        Singleton.instance.SpawnPopUpText(block.player.popUpTextPos.position, "bREAK!", Color.red);

        block.shock.SpawnShockwave(contactPoint, Color.red);

        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }
}
