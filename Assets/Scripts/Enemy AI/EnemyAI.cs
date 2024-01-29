using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This holds things like the stats and booleans of our EnemyAI. As well as the location vector of the player.
//Also holds attack animation data and hurt animation data.
public class EnemyAI : MonoBehaviour
{
    //Declarations for respective things
    private Animator animator;

    [Header("Stats")]
    public float healthMax;
    private float currentHealth;

    public float balanceMax;
    private float currentBalance;

    public float chiDrop;
    private float moveSpeed = 1.0f;
    private Vector3 playerLocation; 

    [Header("States")] //Serializing these fields so that we can inspect them when debugging.
    [SerializeField] private bool preparingAttack; //startup frames of an attack
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isStunned;
    [SerializeField] private bool isInCombat;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isBlocking;

    [Header("Behaviours")]
    public bool canBlock;
    public bool circlesPlayer;

    private void Awake()
    {
        //code to set things like event subscriptions, etc.
        currentHealth = healthMax;
        currentBalance = balanceMax;

        animator = GetComponent<Animator>();
    }

    private void loseHealth(int damage)
    {
        if(currentHealth > 0)
        {
            currentHealth = currentHealth - damage;
            if(currentHealth <= 0)
            {
                //switch EnemyAIStateMachine to "dying" state, stop all coroutines
                return;
            }
            else
            {
                //switch EnemyAIStateMachine to "HitStun" state, stop all coroutines and play hurt animation, play sound effect, etc.
                return;
            }
        }
        else
        {
            //switch EnemyAIStateMachine to "dying" state, stop all coroutines as needed (if we're using coroutines that is)
        }

    }

    private void loseBalance(int balanceDamage)
    {
        if(currentBalance > 0)
        {
            currentBalance = currentBalance - balanceDamage;
            if(currentBalance <=0)
            {
                //switch EnemyAIStateMachine to "BalanceBroken" state, stop all coroutines and play balancebroken animation (probably just a longer stun with a vfx), play sound effect, etc.
                return;
            }
        }
    }

    public void setIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public bool getIsMoving()
    {
        return isMoving;
    }
}
