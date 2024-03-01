using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    PlayerMovement move;
    PlayerCombat combat;
    Rigidbody rb;
    PlayerAOE aoe;
    PlayerLaser laser;
    ClosestObjectFinder finder;

    void Start()
    {
        anim = GetComponent<Animator>();

        move=transform.root.GetComponent<PlayerMovement>();
        combat=transform.root.GetComponent<PlayerCombat>();
        rb=transform.root.GetComponent<Rigidbody>();
        aoe=transform.root.GetComponent<PlayerAOE>();
        laser=transform.root.GetComponent<PlayerLaser>();
        finder=transform.root.GetComponent<ClosestObjectFinder>();
    }

    void Update()
    {
        AnimBlendTree();
        AnimMirror();
        AnimCombat();
    }

    void AnimBlendTree()
    {
        Vector3 moveDir = rb.velocity.normalized;

        float alignmentForward = Vector3.Dot(transform.forward, moveDir);
        float alignmentRight = Vector3.Dot(transform.right, moveDir);

        float velocityRatio = move.velocity/(move.defMoveSpeed*.88f+.001f);

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
        if(finder.target) anim.SetBool("inCombat", true);
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
    
    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
