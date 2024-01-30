using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerMovement move;
    public Animator anim;
    public PlayerWeapon weapon;

    public List<AttackSO> combo;
    public int comboCounter;

    public float attackCooldown=.5f, comboCooldown=1, resetComboAfter=.2f;
    float lastAttackedTime, lastComboEnd;

    public bool isAttacking;

    void Awake()
    {
        move=GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckInput();

        CheckExitAttack();
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

                CancelInvoke("EndCombo");

                isAttacking=true;

                anim.runtimeAnimatorController = combo[comboCounter].animOV; // replace attack animation
                
                anim.CrossFade("Attack", .25f, 2, 0); //anim.Play("Attack", 2, 0); but smoother

                if(weapon)
                {
                    weapon.damage = combo[comboCounter].damage; // replace damage value
                    weapon.knockback = combo[comboCounter].knockback; // replace knockback value
                }

                move.Push(combo[comboCounter].dash, transform.forward, .1f); // push forward

                comboCounter++;

                //if(comboCounter+1 > combo.Count) comboCounter=0; // reset
            }
        }
    }

    void CheckExitAttack()
    { 
        if(anim.GetCurrentAnimatorStateInfo(2).normalizedTime > .9f) // after animation reached 90% done
        {
            if(anim.GetCurrentAnimatorStateInfo(2).IsTag("Attack"))
            {
                Invoke("EndCombo", resetComboAfter); // reset combo after stopping a short while
            }
        } 
    }

    void EndCombo()
    {
        comboCounter=0;

        lastComboEnd = Time.time;

        isAttacking=false;
    }
}
