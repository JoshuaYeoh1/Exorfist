using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    public Player player;
    PlayerMovement move;
    PlayerCombat combat;
    Rigidbody rb;
    PlayerAOE aoe;
    PlayerLaser laser;
    PlayerHeal heal;
    Ragdoller ragdoll;

    void Start()
    {
        anim = GetComponent<Animator>();

        move = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<PlayerCombat>();
        rb = player.GetComponent<Rigidbody>();
        aoe = player.GetComponent<PlayerAOE>();
        laser = player.GetComponent<PlayerLaser>();
        heal = player.GetComponent<PlayerHeal>();
        ragdoll = player.GetComponent<Ragdoller>();
    }

    void Update()
    {
        AnimBlendTree();
        AnimMirror();
        AnimCombat();
    }

    public float baseMoveAnimSpeed=5;

    void AnimBlendTree()
    {
        Vector3 moveDir = rb.velocity.normalized;

        float alignmentForward = Vector3.Dot(transform.forward, moveDir);
        float alignmentRight = Vector3.Dot(transform.right, moveDir);

        float velocityRatio = move.velocity/(baseMoveAnimSpeed*.88f+.001f);

        anim.SetFloat("moveZ", alignmentForward * velocityRatio);
        anim.SetFloat("moveX", alignmentRight * velocityRatio);
    }

    void AnimMirror()
    {
        if(anim.GetFloat("moveX")>=0)
        {
            anim.SetBool("mirror", false);
        }
        else anim.SetBool("mirror", true);
    }

    void AnimCombat()
    {
        if(player.target) anim.SetBool("inCombat", true);
        else anim.SetBool("inCombat", false);
    }
    
    public void AnimRelease(string type)
    {
        combat.AttackRelease(type);
    }
    public void AnimRecover()
    {
        combat.AttackRecover();
    }
    public void AnimFinish()
    {
        combat.AttackFinish();
    }

    public void AOERelease()
    {
        aoe.Release();
    }
    public void AOEFinish()
    {
        aoe.Finish();
    }

    public void LaserRelease()
    {
        laser.Release();
    }
    public void LaserFinish()
    {
        laser.Finish();
    }

    public void HealRelease()
    {
        heal.Release();
    }
    public void HealFinish()
    {
        heal.Finish();
    }

    public Transform footstepLTr, footstepRTr;
    float lastFstTime;
    float fstCooldown=.15f;
    
    public void AnimFootstep(string type="left")
    {
        if(Time.time-lastFstTime > fstCooldown)
        {
            lastFstTime = Time.time;

            Transform footstepTr = type=="left" ? footstepLTr : footstepRTr;

            GameEventSystem.Current.OnFootstep(player.gameObject, type, footstepTr);
        }
    }

    //move to sfx manager later
    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }

    public void AnimRagdoll()
    {
        ragdoll.ToggleRagdoll(true);
    }
}
