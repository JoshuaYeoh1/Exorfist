using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    ClosestObjectFinder finder;
    PlayerCombat combat;
    Rigidbody rb;

    [HideInInspector] public Animator anim;

    void Start()
    {
        player=transform.root.GetComponent<Player>();
        move=transform.root.GetComponent<PlayerMovement>();
        finder=transform.root.GetComponent<ClosestObjectFinder>();
        combat=transform.root.GetComponent<PlayerCombat>();
        rb=transform.root.GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
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

        anim.SetFloat("moveZ", alignmentForward * move.moveSpeed/(move.defMoveSpeed+.01f) * move.dir.magnitude);
        anim.SetFloat("moveX", alignmentRight * move.moveSpeed/(move.defMoveSpeed+.01f) * move.dir.magnitude);
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
        combat.AnimRelease(type);
    }

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
