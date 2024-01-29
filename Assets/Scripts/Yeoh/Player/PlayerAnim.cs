using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    public PlayerMovement move;

    // [Header("SFX")]
    // public AudioClip[] sfxPlayerFst;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 moveDir = move.rb.velocity.normalized;

        float alignmentForward = Vector3.Dot(transform.forward, moveDir);
        float alignmentRight = Vector3.Dot(transform.right, moveDir);

        // 2D blend tree
        anim.SetFloat("moveZ", move.dir.magnitude * alignmentForward);
        anim.SetFloat("moveX", move.dir.magnitude * alignmentRight);
    }

    // public void PlaySfxFootstep()
    // {
    //     Singleton.instance.playSFX(sfxPlayerFst,transform);
    // }
}
