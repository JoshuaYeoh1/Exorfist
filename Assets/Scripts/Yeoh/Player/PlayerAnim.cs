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
    PlayerHeal heal;
    Player player;
    Ragdoller ragdoll;

    void Start()
    {
        anim = GetComponent<Animator>();

        move=transform.root.GetComponent<PlayerMovement>();
        combat=transform.root.GetComponent<PlayerCombat>();
        rb=transform.root.GetComponent<Rigidbody>();
        aoe=transform.root.GetComponent<PlayerAOE>();
        laser=transform.root.GetComponent<PlayerLaser>();
        heal=transform.root.GetComponent<PlayerHeal>();
        player=transform.root.GetComponent<Player>();
        ragdoll=transform.root.GetComponent<Ragdoller>();
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

    public GameObject footstepLVfx, footstepRVfx;
    public Transform footstepLTr, footstepRTr;
    
    float lastFootstepTime, footstepCooldown=.15f;

    public void AnimFootstep(string type="left")
    {
        if(Time.time-lastFootstepTime > footstepCooldown)
        {
            lastFootstepTime = Time.time;

            GameObject step=null;

            if(type=="left")
            {
                step=Instantiate(footstepLVfx, footstepLTr.position, footstepLTr.rotation);
            }
            if(type=="right")
            {
                step=Instantiate(footstepRVfx, footstepRTr.position, footstepLTr.rotation);
            }

            if(step) step.hideFlags = HideFlags.HideInHierarchy;
        }
    }

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }

    public void AnimRagdoll()
    {
        ragdoll.ToggleRagdoll(true);
    }
}
