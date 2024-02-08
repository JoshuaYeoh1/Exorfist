using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    Player player;
    PlayerMovement move;
    ClosestObjectFinder finder;
    PlayerCombat combat;
    Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();

        player=transform.root.GetComponent<Player>();
        move=transform.root.GetComponent<PlayerMovement>();
        finder=transform.root.GetComponent<ClosestObjectFinder>();
        combat=transform.root.GetComponent<PlayerCombat>();
        rb=transform.root.GetComponent<Rigidbody>();
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
        if(player.canLook && finder.target)
        {
            anim.SetBool("inCombat", true);
        }
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

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
