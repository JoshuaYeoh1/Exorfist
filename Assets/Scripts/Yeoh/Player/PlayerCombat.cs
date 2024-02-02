using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    Player player;
    PlayerMovement move;

    public Animator anim;

    public List<AttackSO> combo;
    public int comboCounter;

    public float attackCooldown=.5f, comboCooldown=.5f, resetComboAfter=.5f;
    float lastAttackedTime, lastComboEnd;

    void Awake()
    {
        player=GetComponent<Player>();
        move=GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckInput();
        //CheckExitAttack();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Attack();
    }

    void Attack()
    {
        if(Time.time-lastComboEnd > comboCooldown && comboCounter < combo.Count) // wait for combo cooldown
        {
            if(Time.time-lastAttackedTime > attackCooldown) // wait for attack cooldown
            {
                lastAttackedTime = Time.time;

                player.isAttacking=true;

                //anim.runtimeAnimatorController = combo[comboCounter].animOV; // replace attack animation
                
                anim.CrossFade(combo[comboCounter].animName, .25f, 2, 0); //anim.Play but smoother

                int i = combo[comboCounter].hitboxIndex;

                player.hitboxes[i].damage = combo[comboCounter].damage; // replace damage value
                player.hitboxes[i].knockback = combo[comboCounter].knockback; // replace knockback value

                move.Push(combo[comboCounter].dash, transform.forward, .1f); // push forward

                comboCounter++;

                //if(comboCounter+1 > combo.Count) comboCounter=0; // reset

                if(endingComboRt!=null) StopCoroutine(endingComboRt);
                endingComboRt = StartCoroutine(EndingCombo());
            }
        }
    }

    Coroutine endingComboRt;

    IEnumerator EndingCombo()
    {
        yield return new WaitForSeconds(attackCooldown+resetComboAfter); // reset combo after stopping a short while

        EndCombo();
    }

    void EndCombo()
    {
        if(player.isAttacking)
        {
            player.isAttacking=false;

            comboCounter=0;

            lastComboEnd = Time.time;
        }
    }

    // void CheckExitAttack() // INCONSISTENT
    // { 
    //     if(anim.GetCurrentAnimatorStateInfo(2).normalizedTime>=.7f && !anim.IsInTransition(2)) // after animation is certain % done and not transitioning
    //     {
    //         if(anim.GetCurrentAnimatorStateInfo(2).IsTag("Attack"))
    //         {
    //             Invoke("EndCombo", resetComboAfter); // reset combo after stopping a short while
    //         }
    //     }
    // }
}
