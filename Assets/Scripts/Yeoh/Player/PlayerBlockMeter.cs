using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockMeter : MonoBehaviour
{
    Player player;
    HPManager hp;
    PlayerBlock block;

    [HideInInspector] public bool iframe, regen;
    public float iframeTime=.3f, regenCooldown=3;

    void Awake()
    {
        player=transform.root.GetComponent<Player>();
        hp=GetComponent<HPManager>();
        block=transform.root.GetComponent<PlayerBlock>();
    }

    void Update()
    {
        if(regen!=hp.regen) hp.regen=regen;
    }

    public void Hit(float dmg, float kbForce, Vector3 contactPoint, float speedDebuffMult, float stunTime)
    {
        if(dmg>0)
        {
            hp.Hit(dmg);

            if(hp.hp<=0) block.BlockBreak(dmg, kbForce, contactPoint, speedDebuffMult, stunTime);

            else Singleton.instance.SpawnPopUpText(contactPoint, dmg.ToString(), Color.cyan);
        }
    }

    public void DoIFraming()
    {
        StartCoroutine(iframing());
    }
    IEnumerator iframing()
    {
        iframe=true;
        yield return new WaitForSeconds(iframeTime);
        iframe=false;
    }

    public void CancelRegenCooling()
    {
        regen=false;
        if(regenCoolingRt!=null) StopCoroutine(regenCoolingRt);
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

    public bool EnoughToBlock()
    {
        if(hp.hp>0) return true;
        return false;
    }

    public void Refill(float percent)
    {
        hp.Add(hp.hpMax*percent/100);
    }
}
