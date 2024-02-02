using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Player player;
    PlayerMovement move;
    ClosestObjectFinder finder;
    Rigidbody rb;

    [HideInInspector] public Animator anim;

    void Start()
    {
        player=transform.root.GetComponent<Player>();
        move=transform.root.GetComponent<PlayerMovement>();
        finder=transform.root.GetComponent<ClosestObjectFinder>();
        rb=transform.root.GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimBlendTree();
        AnimCombat();
    }

    void AnimBlendTree()
    {
        Vector3 moveDir = rb.velocity.normalized;

        float alignmentForward = Vector3.Dot(transform.forward, moveDir);
        float alignmentRight = Vector3.Dot(transform.right, moveDir);

        anim.SetFloat("moveZ", move.dir.magnitude * alignmentForward);
        anim.SetFloat("moveX", move.dir.magnitude * alignmentRight);
    }

    void AnimCombat()
    {
        if(player.canLook && finder.target)
        {
            anim.SetBool("inCombat", true);

            if(anim.GetFloat("moveX")>=0) anim.SetBool("mirror", false);

            else anim.SetBool("mirror", true);
        }
        else anim.SetBool("inCombat", false);
    }

    public void BlinkHitbox(int i)
    {
        if(blinkHitboxRt!=null) StopCoroutine(blinkHitboxRt);
        StartCoroutine(BlinkingHitbox(i));
    }

    Coroutine blinkHitboxRt;

    IEnumerator BlinkingHitbox(int i)
    {
        EnableHitbox(i);
        yield return new WaitForSeconds(.2f);
        DisableHitbox(i);
    }

    public void EnableHitbox(int i)
    {
        player.hitboxes[i].ToggleActive(true);
    }

    public void DisableHitbox(int i)
    {
        player.hitboxes[i].ToggleActive(false);
    }

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
