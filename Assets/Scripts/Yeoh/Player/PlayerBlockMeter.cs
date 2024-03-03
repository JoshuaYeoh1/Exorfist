using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockMeter : MonoBehaviour
{
    HPManager hp;
    PlayerBlock block;
    PlayerHurt hurt;

    [HideInInspector] public bool regen;
    public float regenCooldown=3;
    public float blockBreakSpeedDebuffMult=.5f, blockBreakPenaltyStunTime=1;

    void Awake()
    {
        hp=GetComponent<HPManager>();
        block=transform.root.GetComponent<PlayerBlock>();
        hurt=transform.root.GetComponent<PlayerHurt>();
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

<<<<<<< HEAD
    public void Hit(float dmg, float kbForce, Vector3 contactPoint)
=======
    public void Hurt(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint)
>>>>>>> main
    {
        if(!hurt.iframe)
        {
            hp.Hit(dmg);

            if(hp.hp>0) // if not empty yet
            {
<<<<<<< HEAD
                BlockHit(dmg, kbForce, contactPoint);
            }   
            else
            {
                BlockBreak(dmg, kbForce, contactPoint);
=======
                BlockHit(attacker, dmg, kbForce, contactPoint);
            }   
            else
            {
                BlockBreak(attacker, dmg, kbForce, contactPoint);
>>>>>>> main
            }
        }        
    }

<<<<<<< HEAD
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
=======
    void BlockHit(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint)
>>>>>>> main
    {
        block.canBlock=true;

        block.player.anim.CrossFade("block hit", .1f, 4, 0);

        hurt.Knockback(kbForce*block.blockKnockbackResistMult, contactPoint);

        hurt.DoIFraming(hurt.iframeTime, -.5f, .5f, .5f); // flicker cyan

        GameEventSystem.current.OnBlock(block.gameObject, attacker, contactPoint, false, false);




        // move to vfx manager later

        block.PlaySparkVFX(contactPoint, Color.white);

        Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.cyan);

        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }

<<<<<<< HEAD
    void BlockBreak(float dmg, float kbForce, Vector3 contactPoint)
    {
        block.Unblock();
        
        block.hurt.Hit(dmg*.5f, kbForce, contactPoint, blockBreakSpeedDebuffMult, blockBreakPenaltyStunTime);
=======
    void BlockBreak(GameObject attacker, float dmg, float kbForce, Vector3 contactPoint)
    {
        block.Unblock();
        
        hurt.Hurt(attacker, dmg*.5f, kbForce, contactPoint, blockBreakSpeedDebuffMult, blockBreakPenaltyStunTime);
>>>>>>> main

        GameEventSystem.current.OnBlock(block.gameObject, attacker, contactPoint, false, true);




        // move to vfx manager later

        block.PlaySparkVFX(contactPoint, Color.red);

        Singleton.instance.SpawnPopUpText(block.player.popUpTextPos.position, "bREAK!", Color.red);

        block.shock.SpawnShockwave(contactPoint, Color.red);

        //Singleton.instance.PlaySFX(Singleton.instance.sfxSubwoofer, transform.position, false);
    }
}
