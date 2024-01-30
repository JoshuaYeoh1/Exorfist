using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    public PlayerMovement move;
    public PlayerLook look;
    public ClosestObjectFinder finder;

    public List<PlayerWeapon> hitboxes;

    // [Header("SFX")]
    // public AudioClip[] sfxPlayerFst;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimBlendTree();

        AnimCombat();
    }

    void AnimBlendTree()
    {
        Vector3 moveDir = move.rb.velocity.normalized;

        float alignmentForward = Vector3.Dot(transform.forward, moveDir);
        float alignmentRight = Vector3.Dot(transform.right, moveDir);

        anim.SetFloat("moveZ", move.dir.magnitude * alignmentForward);
        anim.SetFloat("moveX", move.dir.magnitude * alignmentRight);
    }

    void AnimCombat()
    {
        if(look.canLook && finder.target)
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
        hitboxes[i].ToggleActive(true);
    }

    public void DisableHitbox(int i)
    {
        hitboxes[i].ToggleActive(false);
    }

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
