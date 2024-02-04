using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//This holds things like the stats and booleans of our EnemyAI. As well as the location vector of the player.
//Also holds attack animation data and hurt animation data.
public class EnemyAI : MonoBehaviour
{
    //Declarations for respective things
    public Animator animator;
    public Rigidbody body;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Stats")]
    public float healthMax;
    private float currentHealth;

    public float balanceMax;
    private float currentBalance;

    public float chiDrop;
    private float moveSpeed = 0.20f;
    public Transform playerTransform;

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

    [Header("Circling Movement Radius")]
    [SerializeField]
    [Range(0, 5)]
    private float circleRadius; //determines radius of CircularMovement

    [Header("Close Player Radius")] //radius for enemy to be considered "near" the player. For melee attack purposes, can be adjusted.
    [SerializeField]
    [Range(0, 5)]
    private float closePlayerRadius;

    [Header("Far player Radius")] //Radius for enemy to be considered "far" from the player. To initiate the "moveCloserTo" function. Or to use for ranged enemies later down the line.
    [SerializeField]
    [Range(0,10)]
    private float farPlayerRadius;

    private void Awake()
    {
        //code to set things like event subscriptions, etc.
        currentHealth = healthMax;
        currentBalance = balanceMax;

        animator = GetComponent<Animator>();
        playerTransform = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void LoseHealth(int damage)
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

    private void LoseBalance(int balanceDamage)
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

    //IsMoving getters/setters
    public void SetIsMoving(bool isMoving)
    {
        this.isMoving = isMoving;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    //IsIdle getters/setters
    public void SetIsIdle(bool isIdle)
    {
        this.isIdle = isIdle;
    }

    public bool GetIsIdle()
    {
        return isIdle;
    }

    //MoveSpeed getters/setters
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }


    //CircleRadius
    public float GetCircleRadius()
    {
        return circleRadius;
    }

    public float GetClosePlayerRadius() { return closePlayerRadius; }

    public float GetFarPlayerRadius() { return farPlayerRadius; }
}
